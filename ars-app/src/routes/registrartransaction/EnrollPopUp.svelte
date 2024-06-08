<script lang='ts'>
	import { onMount } from "svelte";
	import type { CallResultDto } from "../../types/types";
	import { enrollStudent, getSchoolYearList, getSection } from "./repo";
	import type { SchoolSection, SchoolYear } from "./type";
    import { createEventDispatcher } from 'svelte';
    import { loggedInUser, shortcuts } from '$lib/store';

    export let studentId:number;
    export let gradeId:number;
    export let gradeLevel:string;
    export let strandId:number | null;
    
    const dispatch = createEventDispatcher();
  
  const handleClose = () => {
    dispatch('close');
  };
    let errorMessage:string | undefined;
    let successMessage:string | undefined;

    let selectedSection:number;
    let selectedTerm:number;
    let selectedSemester:number | null;
    let schoolSectionListCallResult:CallResultDto<SchoolSection[]> = {
        message:"",
        data: [],
        isSuccess:true,    
        data2:[],
        totalCount: null
    };
    let schoolSections:SchoolSection[] = [];
    let schoolYearCallResult:CallResultDto<SchoolYear[]> = {
        message:"",
        data: [],
        isSuccess:true,    
        data2:[],
        totalCount: null
    };
    let schoolYears:SchoolYear[] = [];
  
    onMount(async () => {
        try {
            schoolSectionListCallResult = await getSection(gradeId, strandId);
            schoolSections = schoolSectionListCallResult.data;
            errorMessage = schoolSectionListCallResult?.message || '';
            schoolYearCallResult = await getSchoolYearList();
            schoolYears = schoolYearCallResult.data;
            errorMessage = schoolYearCallResult?.message || '';
        } catch (err:any ) {
            errorMessage = err.message;
        }
    });

    async function handleSubmit(e:Event){
        e.preventDefault();
        try {
            if($loggedInUser){
             const callResult = await enrollStudent(parseInt($loggedInUser.uid), studentId, selectedSection, selectedTerm, selectedSemester);
             if(callResult.isSuccess){
                successMessage = callResult.message;
                setTimeout(() => {
                    window.location.href = "/registrartransaction";
                }, 1000);
            }else{
                errorMessage = callResult.message;
            }
            }
        } catch (error:any) {
            errorMessage =  error.message;
        } 
    }
  </script>
  
  <div class="modal is-active">
    <div class="modal-background"></div>
    <div class="modal-content ">
      <div class="box has-background-white">
        <h2 class="title is-4 has-text-black">{gradeLevel}</h2>
        {#if schoolSections}
        <div class="field">
          <label class="label has-text-black">Select Section:</label>
          <div class="control">
            <div class="select is-fullwidth">
              <select id="section" class="has-background-white  has-text-black" bind:value={selectedSection}>
                {#each schoolSections as section}
                  <option value={section.id}>{section.sectionName}</option>
                {/each}
              </select>
            </div>
          </div>
        </div>
       
        <div class="field">
          <label class="label  has-text-black">Select Term:</label>
          <div class="control">
            <div class="select is-fullwidth ">
              <select id="term" class="has-background-white  has-text-black" bind:value={selectedTerm}>
                {#each schoolYears as term}
                  <option value={term.id}>{term.fromSchoolTerm}-{term.toSchoolTerm}</option>
                {/each}
              </select>
            </div>
          </div>
        </div>
        {/if}
        {#if gradeId > 13}
        <div class="field">
            <label class="label  has-text-black">Select Semester:</label>
            <div class="control">
              <div class="select is-fullwidth ">
                <select id="term" class="has-background-white  has-text-black" bind:value={selectedSemester}>
                    <option value={1}>Semester 1</option>
                    <option value={2}>Semester 2</option>
                </select>
              </div>
            </div>
          </div>
        {/if}
        <div class="field">
          <div class="control">
            <button class="button is-link" on:click={handleSubmit}>Enroll</button>
          </div>
        </div>
      </div>
    </div>
    <button class="modal-close is-large" aria-label="close" on:click={handleClose}></button>
  </div>
  <style>
    .modal {
      display: flex !important;
      align-items: center;
      justify-content: center;
    }
  </style>