<script lang="ts">
	import type { StudentFormData } from "./type";
	import { faDeleteLeft, faEdit } from "@fortawesome/free-solid-svg-icons";
	import Notification from "$lib/components/Notification.svelte";
	import IconButton from "$lib/components/IconButton.svelte";
	import type { CallResultDto } from "../../types/types";
	import { denyStudent } from "./repo";

    export let students:StudentFormData[] ;
    export let message:string | undefined;
    export let isSuccess:boolean;
    export let goEdit: (studentFormData: StudentFormData) => void; 

    let deny:CallResultDto<object>;
        async function denyUser(id: number) {
    // Show confirmation dialog
    const userConfirmed = confirm("Are you sure you want to deny this student?");
    
    // If the user clicks "OK"
    if (userConfirmed) {
        try {
            // Call denyStudent function and wait for the result
            const deny = await denyStudent(id);
            
            // Display the message from the response
            message = deny.message;
        } catch (error) {
            // Handle any errors that occur during the API call
            console.error("Error denying student:", error);
            message = "An error occurred while denying the student.";
        }
    } else {
        // User clicked "Cancel", do nothing
    }
}

</script>

<div style="overflow: auto;">
    <table class="table is-fullwidth is-bordered has-background-white my-custom-table">
        <thead>
          <tr>
            <th class="has-text-black">LRN</th>
            <th class="has-text-black">Name</th>
            <th class="has-text-black">Email</th>
            <th class="has-text-black">Actions</th>
          </tr>
        </thead>
        <tbody>
            {#each students as user}
                <tr>
                    <td>{user.lrn}</td>
                    <td>{user.firstName} {user.middleName || ''} {user.lastName}</td>
                    <td>{user.email}</td>
                    <td class="has-text-centered">
                      <IconButton class="button-blue has-text-white" icon={faEdit} on:click={() => goEdit(user)}>View/Edit</IconButton>
                      <IconButton class="button-red has-text-white"  icon={faDeleteLeft} on:click={() => denyUser(user.id)}>Delete</IconButton>

                    </td>
                </tr>
            {/each}    
        </tbody>
    </table>
</div>


{#if message != ''}
  <Notification errorMessage={message} successMessage={message}></Notification>
{/if}

<style>
    .my-custom-table {
        color: black;
    }
    .my-custom-table th,
    .my-custom-table td {
        color: black;
    }
</style>