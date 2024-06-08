
<script lang="ts">
	import Icon from "$lib/components/Icon.svelte";
	import { faArrowLeft } from "@fortawesome/free-solid-svg-icons";
	import type { CallResultDto } from "../../../types/types";
	import Notification from "$lib/components/Notification.svelte";
	import type {  SchoolYear } from "../type";
	import { addSchoolYear } from "../repo";

    let schoolyears: SchoolYear = {
        id: 0,
  fromSchoolTerm: "",
  toSchoolTerm: "",
  isActive: false
    };

    let errorMessage:string | undefined;
    let successMessage:string | undefined;

    async function handleSubmit(e:Event){
        e.preventDefault();

        try {
            const callResult:CallResultDto<object> = await addSchoolYear(schoolyears);
            if(callResult.isSuccess){
                successMessage = callResult.message;
                setTimeout(() => {
                    window.location.href = "/schoolterm";
                }, 2000);
            }else{
                errorMessage = callResult.message;
            }
        } catch (error:any) {
            errorMessage = error.message;
        }
    } 

</script>
<div class="is-flex is-align-items-center">
    <a class="button is-link" href="/schoolterm">
        <Icon icon={faArrowLeft}/>
    </a>      
    <h1 class="subtitle ml-2 has-text-black">Add School Term</h1>
</div>
<!-- on:submit={handleSubmit} -->
<form class="mx-auto is-full" on:submit={handleSubmit}>
    <fieldset>
        <legend class="has-text-black">Add School Term</legend>

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
            <button class="button is-link" >Add</button>
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

  </style>