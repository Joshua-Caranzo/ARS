<script lang='ts'>
	import { onMount } from "svelte";
	import type { CallResultDto } from "../../types/types";
	import { changeSection, enrollStudent, getCurrentSchoolTerm, getCurrentSection, getGradeLevels, getSchoolYearList, getSection, getStrands } from "./repo";
	import type { CurrentSection, GradeLevel, SchoolSection, SchoolYear, StrandDto } from "./type";
    import { createEventDispatcher } from 'svelte';
    import { loggedInUser, shortcuts } from '$lib/store';

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

    let currentSectionListCallResult:CallResultDto<CurrentSection>;
    let currentSection: CurrentSection;
    let sy:number;

    let gradeLevelCallResult:CallResultDto<GradeLevel[]> = {
        message:"",
        data: [],
        isSuccess:true,    
        data2:[],
        totalCount: null
    };
    let gradeLevels:GradeLevel[] = [];
    let currentGradeLevel:GradeLevel | null;
    let sectionId:number;
    const refresh = async () => {
        try {
            if($loggedInUser)
            {
            const currentsyCallResult = await getCurrentSchoolTerm();
            sy = currentsyCallResult.data;
            currentSectionListCallResult = await getCurrentSection(studentId, sy);
            currentSection = currentSectionListCallResult.data;
            schoolSectionListCallResult = await getSection(parseInt($loggedInUser.uid), currentSection.gradeLevelId, selectedStrand);
            schoolSections = schoolSectionListCallResult.data.filter(section => section.id !== currentSection.sectionId);
            errorMessage = schoolSectionListCallResult?.message || '';
            gradeLevelCallResult = await getGradeLevels();
            gradeLevels = gradeLevelCallResult.data;
            currentGradeLevel = gradeLevels.find(grade => grade.id === currentSection.gradeLevelId) || null;
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
             const callResult = await changeSection( studentId, sy,sectionId);
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
  </script>
  

  <div class="modal is-active">
    <div class="modal-background"></div>
    <div class="modal-content ">
      <div class="box has-background-white">
        {#if currentSection}
        <h2 class="title is-4 has-text-black">Current Section: {currentGradeLevel?.level} - {currentSection.sectionName}</h2>
        {/if}
        <div class="field">
          <label class="label has-text-black">Select Section:</label>
          <div class="control">
            <div class="select is-fullwidth">
              <select id="section" class="has-background-white  has-text-black" bind:value={sectionId}>
                {#each schoolSections as section}
                  <option value={section.id}>{section.sectionName}</option>
                {/each}
              </select>
            </div>
          </div>
        </div>
        <div class="field is-flex is-justify-content-flex-end">
          <div class="control">
            <button class="button button-blue" on:click={handleSubmit}>Confirm Transfer</button>
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