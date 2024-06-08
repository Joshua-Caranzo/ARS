<script lang="ts">
	import type { CallResultDto } from "../../../../types/types";
	import Icon from "$lib/components/Icon.svelte";
	import { faArrowLeft } from "@fortawesome/free-solid-svg-icons";
	import { onMount } from "svelte";
	import Notification from "$lib/components/Notification.svelte";
	import type { GradeLevel, SchoolSection, StrandDto } from "../../type";
	import { editSection, getGradeLevels, getSectionById, getStrands } from "../../repo";

    export let data: {   
        id:number
    };

    let errorMessage:string | undefined;
    let successMessage:string | undefined;

    let result: CallResultDto<SchoolSection>;
    let schoolSection: SchoolSection= {
        id: 0,
    gradeLevelId: 0,
    sectionName: "",
    schoolId: 0,
    strandId: 0
    };

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
    let assignedId : number = 0;
    let strandId : number = 0;

    onMount(async () => {
        try {
            result = await getSectionById(data.id);
            
            schoolSection = {
                ...result.data,
                id: data.id,
            };
            strandId = schoolSection.strandId ?? 0;
            if(!result.isSuccess){
                errorMessage = result.message;
            }
            
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
        schoolSection = {...schoolSection, id: data.id};
        schoolSection.schoolId = assignedId;
        schoolSection.strandId = strandId;
        try {
            const callResult:CallResultDto<object> = await editSection(schoolSection);

            if(callResult.isSuccess){
                successMessage = callResult.message;
                setTimeout(() => {
                    window.location.href = "/adminsection";
                }, 4000);
            }else{
                errorMessage = callResult.message;
            }
        } catch (error:any) {
            errorMessage =  error.message;
        } 
    }

</script>

<div class="container is-narrow">
    {#if result?.isSuccess}
        <div class="is-flex is-align-items-center mb-3">
            <a class="button is-link" href="/adminsection">
                <Icon icon={faArrowLeft}/>
            </a>
            <h1 class="subtitle ml-2 has-text-black">Edit Section</h1>        
        </div>

        <form class=" mx-auto" on:submit={handleSubmit}>
            <fieldset>
                <legend>Edit Section</legend>
                <div class="columns">
                    <div class="column is-half">
                        <label class="label has-text-black">Grade Level</label>
                        <select id="gradeLEvel" class="input has-background-white has-text-black" bind:value={schoolSection.gradeLevelId}>
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
                        <select id="section" class="input has-background-white has-text-black" bind:value={strandId}>
                            <option value={0}>-- SELECT --</option>
                            {#each strands as strand}
                                <option value={strand.id}>{strand.strandAbbrev ? strand.strandAbbrev : strand.strandName}</option>
                            {/each}
                        </select>
                    </div>  
                </div>
         {/if}

                <div class="field">
                    <div class="control">
                        <button class="button is-link">Save</button>
                    </div>
                </div>
            </fieldset>
        </form>
    {/if}
</div>

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
</style>

