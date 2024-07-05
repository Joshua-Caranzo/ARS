
<script lang="ts">
	import Icon from "$lib/components/Icon.svelte";
	import { faArrowLeft } from "@fortawesome/free-solid-svg-icons";
	import type { CallResultDto } from "../../../types/types";
	import Notification from "$lib/components/Notification.svelte";
	import type { School } from "../type";
	import { addSchool } from "../repo";

    let schools: School = {
        id:0,
        schoolName: "",
        schoolAcronym: "",
        schoolAddress: "",
        schoolEmail: "",
        schoolContactNum: "",
        isActive:true,
        imagePath:null
    };

    let errorMessage:string | undefined;
    let successMessage:string | undefined;

    async function handleSubmit(e:Event){
        e.preventDefault();

        try {
            const callResult:CallResultDto<object> = await addSchool(schools);

            if(callResult.isSuccess){
                successMessage = callResult.message;
                setTimeout(() => {
                    window.location.href = "/school";
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
    <a class="button button-blue" href="/school">
        <Icon icon={faArrowLeft}/>
    </a>      
    <h1 class="subtitle ml-2 has-text-black">Add School</h1>
</div>
<!-- on:submit={handleSubmit} -->
<form class="mx-auto is-full" on:submit={handleSubmit}>
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