<script lang='ts'>
	import { onMount } from "svelte";
	import type { CallResultDto } from "../../types/types";
	import { changeStrand, enrollStudent, getCurrentSchoolTerm, getCurrentStrand, getGradeLevels, getSchoolYearList, getSection, getStrands } from "./repo";
	import type { CurrentStrand, GradeLevel, SchoolSection, SchoolYear, StrandDto } from "./type";
    import { createEventDispatcher } from 'svelte';
    import { loggedInUser, shortcuts } from '$lib/store';
	import { faArrowRight } from "@fortawesome/free-solid-svg-icons";
	import Icon from "$lib/components/Icon.svelte";

    export let studentId:number;
    
    const dispatch = createEventDispatcher();
  
  const handleClose = () => {
    dispatch('close');
  };
    let errorMessage:string | undefined;
    let successMessage:string | undefined;

    let schoolSectionListCallResult:CallResultDto<SchoolSection[]> = {
        message:"",
        data: [],
        isSuccess:true,    
        data2:[],
        totalCount: null
    };
    let schoolSections:SchoolSection[] = [];
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
    let sy:number;
    let studentStrand:CurrentStrand;
    let currentStrand: StrandDto | null = null;
    let changeStrandId:number;
    let selectedSection:number;

    const refresh = async () => {
        try {
            if($loggedInUser)
            {
            const currentsyCallResult = await getCurrentSchoolTerm();
            sy = currentsyCallResult.data;
            const strandCallResult = await getCurrentStrand(studentId, sy);
            studentStrand = strandCallResult.data;
            errorMessage = strandCallResult?.message || '';
            const strandsCallResult = await getStrands();
            strands = strandsCallResult.data;
            errorMessage = gradeLevelCallResult?.message || '';    
            currentStrand = strands.find(strand => strand.id === studentStrand.strandId) || null;
            schoolSectionListCallResult = await getSection(parseInt($loggedInUser.uid), studentStrand.gradeLevelId, changeStrandId);
            schoolSections = schoolSectionListCallResult.data;
            } 
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
             const callResult = await changeStrand(studentId, selectedSection, changeStrandId, sy);
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
    changeStrandId = parseInt(selectElement.value);
    refresh(); 
    };
  </script>
  

  <div class="modal is-active">
    <div class="modal-background"></div>
    <div class="modal-content ">
      <div class="box has-background-white">
        <h2 class="title is-4 has-text-black">Current Strand: {currentStrand?.strandAbbrev ?? currentStrand?.strandName}</h2>
        
        <div class="field">
            <label class="label  has-text-black">Select Strand:</label>
            <div class="control">
              <div class="select is-fullwidth ">
                <select id="term" class="has-background-white  has-text-black" on:change={handleStrandChange} bind:value={changeStrandId}>
                    {#each strands as strand}
                    <option value={strand.id}>{strand.strandAbbrev || strand.strandName}</option>
                  {/each}
                </select>
              </div>
            </div>
          </div>
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
        <div class="field is-flex is-justify-content-flex-end">
          <div class="control">
            <button class="button button-blue" on:click={handleSubmit}>Save Changes</button>
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

    .button-blue
	{
		background-color: #063F78;
        color:white;
	}
  </style>