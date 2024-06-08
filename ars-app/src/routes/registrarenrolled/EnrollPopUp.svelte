<script lang='ts'>
	import { onMount } from "svelte";
	import type { CallResultDto } from "../../types/types";
	import { enrollStudent, getGradeLevels, getSchoolYearList, getSection, getStrands } from "./repo";
	import type { GradeLevel, SchoolSection, SchoolYear, StrandDto } from "./type";
    import { createEventDispatcher } from 'svelte';
    import { loggedInUser, shortcuts } from '$lib/store';
	import { faArrowRight } from "@fortawesome/free-solid-svg-icons";
	import Icon from "$lib/components/Icon.svelte";

    export let studentId:number;
    export let gradeId:number;
    export let gradeLevel:string;
    
    const dispatch = createEventDispatcher();
  
  const handleClose = () => {
    dispatch('close');
  };
    let errorMessage:string | undefined;
    let successMessage:string | undefined;

    let selectedSection:number = 0;
    let selectedTerm:number = 0;
    let selectedStrand:number | null = null;
    let selectedSemester:number | null = null;
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
    let gradeLevelCallResult:CallResultDto<GradeLevel[]> = {
        message:"",
        data: [],
        isSuccess:true,    
        data2:[],
        totalCount: null
    };
    let gradeLevels:GradeLevel[] = [];
    let newGradelevel:GradeLevel | null;
    let strands:StrandDto[] = [];

    const refresh = async () => {
        try {
       
            schoolSectionListCallResult = await getSection(gradeId, selectedStrand);
            schoolSections = schoolSectionListCallResult.data;
            errorMessage = schoolSectionListCallResult?.message || '';
            schoolYearCallResult = await getSchoolYearList();
            schoolYears = schoolYearCallResult.data;
            errorMessage = schoolYearCallResult?.message || '';
            gradeLevelCallResult = await getGradeLevels();
            gradeLevels = gradeLevelCallResult.data;
            const strandsCallResult = await getStrands();
            strands = strandsCallResult.data;
            console.log(strands);
            errorMessage = gradeLevelCallResult?.message || '';
          
            newGradelevel = gradeLevels.find(grade => grade.id === gradeId) || null;
            
        } catch (err:any ) {
            errorMessage = err.message;
        }
    };
  
    onMount(async () => {
        await refresh();
    });

    async function handleSubmit(e:Event){
        e.preventDefault();
        try {
            if($loggedInUser){
             const callResult = await enrollStudent(parseInt($loggedInUser.uid), studentId, selectedSection, selectedTerm, selectedSemester, selectedStrand);
             if(callResult.isSuccess){
                successMessage = callResult.message;
                setTimeout(() => {
                    window.location.href = "/registrarenrolled";
                }, 1000);
            }else{
                errorMessage = callResult.message;
            }
            }
        } catch (error:any) {
            errorMessage =  error.message;
        } 
    }

    const handleStrandChange = (event: Event) => {
    const selectElement = event.target as HTMLSelectElement;
    selectedStrand = parseInt(selectElement.value);
    refresh(); // Optionally, you can call the refresh function here if you want to update the data immediately after the strand selection changes.
};
  </script>
  

  <div class="modal is-active">
    <div class="modal-background"></div>
    <div class="modal-content ">
      <div class="box has-background-white">
        <h2 class="title is-4 has-text-black">{gradeLevel} <Icon icon={faArrowRight} className="mt-2"> </Icon> {newGradelevel?.level}</h2>
        {#if gradeId > 13}
        <div class="field">
            <label class="label  has-text-black">Select Strand:</label>
            <div class="control">
              <div class="select is-fullwidth ">
                <select id="term" class="has-background-white  has-text-black" on:change={handleStrandChange} bind:value={selectedStrand}>
                    {#each strands as strand}
                    <option value={strand.id}>{strand.strandAbbrev || strand.strandName}</option>
                  {/each}
                </select>
              </div>
            </div>
          </div>{/if}
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