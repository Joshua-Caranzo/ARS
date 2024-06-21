<script lang="ts">
	import type { StudentFormData } from "./type";
	import { faEdit } from "@fortawesome/free-solid-svg-icons";
	import Notification from "$lib/components/Notification.svelte";
	import IconButton from "$lib/components/IconButton.svelte";

    export let students:StudentFormData[] ;
    export let message:string | undefined;
    export let isSuccess:boolean;
    export let goEdit: (studentFormData: StudentFormData) => void; 
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
                        <IconButton icon={faEdit} on:click={() => goEdit(user)} label="Edit"/>
                    </td>
                </tr>
            {/each}    
        </tbody>
    </table>
</div>


{#if message != ''}
  <Notification errorMessage={message}></Notification>
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