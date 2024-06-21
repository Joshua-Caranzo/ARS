<script lang="ts">
	import type { CallResultDto } from "../../../types/types";
	import Icon from "$lib/components/Icon.svelte";
	import { faArrowLeft } from "@fortawesome/free-solid-svg-icons";
	import { createEventDispatcher, onMount } from "svelte";
	import Notification from "$lib/components/Notification.svelte";
	import type { School } from "../type";
	import { editSchool, getSchoolById } from "../repo";

    export let s: School;
    const dispatch = createEventDispatcher();

    let errorMessage:string | undefined;
    let successMessage:string | undefined;
    let result: CallResultDto<School>;
    let schools: School = {
        id:0,
        schoolName: "",
        schoolAcronym: "",
        schoolAddress: "",
        schoolEmail: "",
        schoolContactNum: "",
        isActive:true,
        imagePath: null
    };

    onMount(async () => {
        try {
            result = await getSchoolById(s.id);
            
            schools = {
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
        schools = {...schools,id: s.id};
        try {
            const callResult:CallResultDto<object> = await editSchool(schools);

            if(callResult.isSuccess){
                successMessage = callResult.message;
                setTimeout(() => {
                    window.location.href = "/school";
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
            <h1 class="subtitle ml-2 has-text-black">Edit School</h1>        
        </div>

        <form class=" mx-auto" on:submit={handleSubmit}>
            <fieldset>
                <legend class="has-text-black">Add School</legend>
        
                <div class="columns">
                    <div class="column is-half">
                        <label class="label has-text-black">School Name</label>
                        <div class="control">
                            <input class="input has-background-white has-text-black" type="text" bind:value={schools.schoolName} required>
                        </div>
                    </div>
                    <div class="column is-half">
                        <label class="label has-text-black">School Acronym</label>
                        <div class="control">
                            <input class="input has-background-white has-text-black" type="text" bind:value={schools.schoolAcronym} required>
                        </div>
                    </div>
                </div> 
                <div class="columns">
                    <div class="column is-half">
                        <label class="label has-text-black">School Address</label>
                        <div class="control">
                            <input class="input has-background-white has-text-black" type="text" bind:value={schools.schoolAddress} required>
                        </div>
                    </div>
                    <div class="column is-half">
                        <label class="label has-text-black">School Email</label>
                        <div class="control">
                            <input class="input has-background-white has-text-black" type="email" bind:value={schools.schoolEmail} required>
                        </div>
                    </div>
                </div>
            
                <div class="columns">
                    <div class="column is-half">
                        <label class="label has-text-black">Contact Number</label>
                        <div class="control">
                            <input class="input has-background-white has-text-black" type="text" bind:value={schools.schoolContactNum} required>
                        </div>
                    </div>
                </div>
                
                <div class="block is-flex">
                    <div class="is-flex mr-4">
                        <label class="mr-2">Active</label>
                        <input type="checkbox" bind:checked={schools.isActive}>
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

