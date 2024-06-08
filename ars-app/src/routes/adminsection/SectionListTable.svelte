<script lang="ts">
	import Icon from "$lib/components/Icon.svelte";
	import { faEdit } from "@fortawesome/free-solid-svg-icons";
	import Notification from "$lib/components/Notification.svelte";
	import type { SchoolSectionDto } from "./type";

    export let sections:SchoolSectionDto[] = [];
    export let message:string | undefined;
    export let isSuccess:boolean;

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
                        <a class="button is-link" href={`/adminsection/edit/${section.id}`}>
                            <Icon icon={faEdit} className="mr-2"/>
                            Edit
                        </a>
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