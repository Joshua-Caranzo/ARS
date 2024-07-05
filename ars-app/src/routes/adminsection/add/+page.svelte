
<script lang="ts">
	import Icon from "$lib/components/Icon.svelte";
	import { faArrowLeft } from "@fortawesome/free-solid-svg-icons";
	import type { CallResultDto } from "../../../types/types";
	import Notification from "$lib/components/Notification.svelte";
	import { onMount } from "svelte";
    import { loggedInUser, shortcuts } from '$lib/store';
	import type { GradeLevel, SchoolSection, SchoolSectionDto, StrandDto } from "../type";
	import { addSection, getGradeLevels, getStrands } from "../repo";
	import { userIsInRole } from "$lib/auth/auth";
	import { addSchool } from "../../school/repo";

    let schoolSection: SchoolSection= {
        id: 0,
    gradeLevelId: 0,
    sectionName: "",
    schoolId: 0,
    strandId: 0
    };

    let errorMessage:string | undefined;
    let successMessage:string | undefined;
    let gradeLevelListCallResult:CallResultDto<GradeLevel[]> = {
        message:"",
        data: [],
        isSuccess:true,    
        data2:[],
        totalCount: null
    };
    let gradeLevels:GradeLevel[] = [];
    let strandListCallResult:CallResultDto<StrandDto[]> = {
        message:"",
        data: [],
        isSuccess:true,    
        data2:[],
        totalCount: null
    };
    let strands:StrandDto[] = [];
       

        onMount(async () => {
        try {
            gradeLevelListCallResult = await getGradeLevels();
            gradeLevels = gradeLevelListCallResult.data;
            errorMessage = gradeLevelListCallResult?.message || '';
            strandListCallResult = await getStrands();
            strands = strandListCallResult.data;
            errorMessage = strandListCallResult?.message || '';
        } catch (err:any ) {
            errorMessage = err.message;
        }
    });

    async function handleSubmit(e:Event){
        e.preventDefault();

        try {
            if($loggedInUser){
            const callResult:CallResultDto<object> = await addSection(schoolSection, parseInt($loggedInUser.uid));

            if(callResult.isSuccess){
                successMessage = callResult.message;
                setTimeout(() => {
                    window.location.href = "/adminsection";
                }, 4000);
            }
            else{
                errorMessage = callResult.message;
            }
        }
        } catch (error:any) {
            errorMessage = error.message;
        }
    }

</script>
<div class="is-flex is-align-items-center">
    <a class="button button-blue" href="/adminsection">
        <Icon icon={faArrowLeft}/>
    </a>      
    <h1 class="subtitle ml-2 has-text-black">Add Section</h1>
</div>

<form class="mx-auto is-full" on:submit={handleSubmit}>
    <fieldset>
        <legend class="has-text-black">Add Section</legend>

        <div class="columns">
            <div class="column is-half">
                <label class="label has-text-black">Grade Level</label>
                <select class="input has-background-white has-text-black" bind:value={schoolSection.gradeLevelId}>
                    <option value={0}>-- SELECT --</option>
                    {#each gradeLevels as gradeLevel}
                        <option value={gradeLevel.id}>{gradeLevel.level}</option>
                    {/each}
                </select>
            </div>
            <div class="column is-half">
                <label class="label has-text-black">Section Name</label>
                <div class="control">
                    <input class="input has-background-white has-text-black" type="text" bind:value={schoolSection.sectionName} required>
                </div>
            </div>
        </div>
    {#if schoolSection.gradeLevelId == 14 || schoolSection.gradeLevelId == 15}
        <div class="columns">
            <div class="column is-half">
                <label class="label has-text-black">Strand</label>
                <select class="input has-background-white has-text-black" bind:value={schoolSection.strandId}>
                    <option value={0}>-- SELECT --</option>
                    {#each strands as strand}
                        <option value={strand.id}>   {strand.strandAbbrev ? strand.strandAbbrev : strand.strandName}</option>
                    {/each}
                </select>
            </div>  
        </div>
    {/if}
        <div class="field">
            <div class="control">
            <button class="button button-blue is-pulled-right" >Add</button>
            </div>
        </div>
    </fieldset>
</form>

{#if errorMessage != '' || successMessage != ''}
  <Notification {errorMessage} {successMessage}></Notification>
{/if}


  <style>
    .max-w-md{
        max-width: 28rem;
    }	

    .max-w-lg{
        max-width: 32rem;
    }
	
    .max-w-xl{
        max-width: 36rem;
    }	

    fieldset 
    {
        border: 1px solid #dbdbdb;
        border-radius: 5px;
        padding: 10px;
        margin-bottom: 10px;
    }

    legend 
    {
        font-size: 1.2rem;
        font-weight: bold;
        margin-bottom: 5px;
    }
    .button-blue
	{
		background-color: #063F78;
        color:white;
	}
  </style>