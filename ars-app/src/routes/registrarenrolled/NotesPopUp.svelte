<script lang='ts'>
	import { onMount } from "svelte";
	import type { CallResultDto } from "../../types/types";
	import { addNotes, enrollStudent, getGradeLevels, getNotes, getSchoolYearList, getSection, getStrands } from "./repo";
	import type { GradeLevel, SchoolSection, SchoolYear, StrandDto } from "./type";
    import { createEventDispatcher } from 'svelte';
    import { loggedInUser, shortcuts } from '$lib/store';
	import { faArrowRight } from "@fortawesome/free-solid-svg-icons";
	import Icon from "$lib/components/Icon.svelte";
	import IconButton from "$lib/components/IconButton.svelte";

    export let studentId:number;
    
    let notesCallResult:CallResultDto<string>;
    let notes:string = "";

    const dispatch = createEventDispatcher();
  
  const handleClose = () => {
    dispatch('close');
  };
    let errorMessage:string | undefined;
    let successMessage:string | undefined;

    const refresh = async () => {
        try {
            if($loggedInUser)
            {
                notesCallResult = await getNotes(studentId);
                notes = notesCallResult.data;
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
             const callResult = await addNotes(studentId,notes);
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
        <h2 class="title is-4 has-text-black">Notes:</h2>
        <div class="modal-body">
            <div class="field"><textarea class="textarea is-medium has-background-white has-text-black" bind:value={notes}></textarea></div>
        </div>
        <div class="field is-flex is-justify-content-flex-end mt-2">
            <div class="control">
              <IconButton class="button button-blue" on:click={handleSubmit}>Save</IconButton>
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