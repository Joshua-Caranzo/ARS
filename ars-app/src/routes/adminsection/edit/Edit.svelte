<script lang="ts">
    import { onMount, createEventDispatcher } from "svelte";
    import type { CallResultDto } from "../../../types/types";
    import type { EditSchoolSectionDto, GradeLevel, SchoolSection, SchoolSectionDto, StrandDto } from "../type";
    import Icon from "$lib/components/Icon.svelte";
    import { faArrowLeft } from "@fortawesome/free-solid-svg-icons";
    /* import { editUser, getSchool, getUserById, getUserType } from '../repo'; */
    import Notification from "$lib/components/Notification.svelte";
    import { goto } from "$app/navigation";
	import { editSection, getGradeLevels, getSectionById, getStrands } from "../repo";

    const dispatch = createEventDispatcher();

    export let s: SchoolSectionDto;

    let section: EditSchoolSectionDto = {
        id: 0,
        gradeLevelId: 0,
        sectionName: "",
        schoolId: 0
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

    let errorMessage: string | undefined;
    let successMessage: string | undefined;

    let result: CallResultDto<SchoolSection>;

    onMount(async () => {
        try {
            result = await getSectionById(s.id);
            section = {
                ...result.data,
                id: s.id,
            };
            if (!result.isSuccess) {
                errorMessage = result.message;
            }
            gradeLevelListCallResult = await getGradeLevels();
            gradeLevels = gradeLevelListCallResult.data;
            errorMessage = gradeLevelListCallResult?.message || '';
            strandListCallResult = await getStrands();
            strands = strandListCallResult.data;
            errorMessage = strandListCallResult?.message || '';
            /* assignedId = user.assignedSchoolId; */
        } catch (err: any) {
            errorMessage = err.message;
        }
    });

    async function handleSubmit(e: Event) {
        e.preventDefault();
        section = { ...section, id: s.id };

        if (!(section.gradeLevelId == 14 || section.gradeLevelId == 15)){
            section.strandId = null;
        }

        try {
            const callResult: CallResultDto<object> = await editSection(section);

            if (callResult.isSuccess) {
                successMessage = callResult.message;
                setTimeout(() => {
                    handleClose();
                }, 2000);
            } else {
                errorMessage = callResult.message;
            }
        } catch (error: any) {
            errorMessage = error.message;
        }
    }

    function handleClose() {
        dispatch('close');
    }
</script>

<div class="container is-narrow">
    {#if result?.isSuccess}
        <div class="is-flex is-align-items-center mb-3">
            <button class="button is-link" on:click={handleClose}>
                <Icon icon={faArrowLeft}/>
            </button>
            <h1 class="subtitle ml-2 has-text-black">Edit Section</h1>
        </div>

        <form class=" mx-auto" on:submit={handleSubmit}>
            <fieldset>
                <legend>Edit Section</legend>
                <div class="columns">
                        <div class="column is-half">
                            <label class="label has-text-black">Grade Level</label>
                            <select class="input has-background-white has-text-black" bind:value={section.gradeLevelId}>
                                <option value={0}>-- SELECT --</option>
                                {#each gradeLevels as gradeLevel}
                                    <option value={gradeLevel.id}>{gradeLevel.level}</option>
                                {/each}
                            </select>
                        </div>
                        <div class="column is-half">
                            <label class="label has-text-black">Section Name</label>
                            <div class="control">
                                <input class="input has-background-white has-text-black" type="text" bind:value={section.sectionName} required>
                            </div>
                        </div>
                    </div>
                {#if section.gradeLevelId == 14 || section.gradeLevelId == 15}
                    <div class="columns">
                        <div class="column is-half">
                            <label class="label has-text-black">Strand</label>
                            <select class="input has-background-white has-text-black" bind:value={section.strandId}>
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
                        <button class="button is-link" >Save</button>
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
    .max-w-md {
        max-width: 28rem;
    }

    .max-w-lg {
        max-width: 32rem;
    }

    .max-w-xl {
        max-width: 36rem;
    }

    fieldset {
        border: 1px solid #dbdbdb;
        border-radius: 5px;
        padding: 10px;
        margin-bottom: 10px;
    }

    legend {
        font-size: 1.2rem;
        font-weight: bold;
        margin-bottom: 5px;
    }
</style>