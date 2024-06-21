<script lang="ts">
	import type { CallResultDto } from "../../../types/types";
	import Icon from "$lib/components/Icon.svelte";
	import { faArrowLeft } from "@fortawesome/free-solid-svg-icons";
	import { createEventDispatcher, onMount } from "svelte";
	import Notification from "$lib/components/Notification.svelte";
	import type { SchoolYear } from "../type";
	import { editSchoolYear, getSchoolYearById } from "../repo";

    export let s: SchoolYear;

    const dispatch = createEventDispatcher();

    let errorMessage:string | undefined;
    let successMessage:string | undefined;
    let result: CallResultDto<SchoolYear>;
    let schoolyears: SchoolYear = {
        id: 0,
        fromSchoolTerm: "",
        toSchoolTerm: "",
        isActive: false
    };

    onMount(async () => {
        try {
            result = await getSchoolYearById(s.id);
            
            schoolyears = {
                ...result.data,
                id: s.id,
            };
            if(!result.isSuccess){
                errorMessage = result.message;
            }
        } catch (err:any ) {
            errorMessage = err.message;
        }
    });

    async function handleSubmit(e:Event){
        e.preventDefault();
        schoolyears = {...schoolyears,id: s.id};
        try {
            const callResult:CallResultDto<object> = await editSchoolYear(schoolyears);

            if(callResult.isSuccess){
                successMessage = callResult.message;
                setTimeout(() => {
                    window.location.href = "/schoolterm";
                }, 2000);
            }else{
                errorMessage = callResult.message;
            }
        } catch (error:any) {
            errorMessage =  error.message;
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
            <h1 class="subtitle ml-2 has-text-black">Edit School Term</h1>        
        </div>

        <form class=" mx-auto" on:submit={handleSubmit}>
            <fieldset>
                <legend class="has-text-black">Edit School Term</legend>
        
                <div class="columns">
                    <div class="column is-half">
                        <label class="label has-text-black">From</label>
                        <div class="control">
                            <input class="input has-background-white has-text-black" type="text" bind:value={schoolyears.fromSchoolTerm} required>
                        </div>
                    </div>
                    <div class="column is-half">
                        <label class="label has-text-black">To</label>
                        <div class="control">
                            <input class="input has-background-white has-text-black" type="text" bind:value={schoolyears.toSchoolTerm} required>
                        </div>
                    </div>
                </div> 
                <div class="columns">
                    <div class="column is-half">
                        <label class="label has-text-black">IsActive</label>
                        <div class="control">
                          <label class="checkbox">
                            <input type="checkbox" bind:checked={schoolyears.isActive}>
                            Active
                          </label>
                        </div>
                    </div>
                    
                </div>

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

