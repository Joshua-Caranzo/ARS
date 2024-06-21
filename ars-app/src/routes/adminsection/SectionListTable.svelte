<script lang="ts">
	import { faEdit } from "@fortawesome/free-solid-svg-icons";
	import Notification from "$lib/components/Notification.svelte";
	import type { SchoolSectionDto } from "./type";
	import IconButton from "$lib/components/IconButton.svelte";

    export let sections:SchoolSectionDto[] = [];
    export let message:string | undefined;
    export let isSuccess:boolean;
    export let goEdit: (user: SchoolSectionDto) => void;
</script>

<div style="overflow: auto;">
    <table class="table is-fullwidth is-bordered has-background-white my-custom-table">
        <thead>
          <tr>
            <th>Grade Level</th>
            <th>Section Name</th>
            <th>Strand</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
            {#each sections as section}
                <tr>
                    <td>{section.level}</td>
                    <td>{section.sectionName}</td>
                    <td>{section.strandName || "Not Applicable"}</td>
                    <td class="has-text-centered">
                        <IconButton icon={faEdit} on:click={() => goEdit(section)} label="Edit"/>
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