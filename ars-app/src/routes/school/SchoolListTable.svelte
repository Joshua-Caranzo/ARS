<script lang="ts">
    import type { School } from "./type";
    import Icon from "$lib/components/Icon.svelte";
    import { faEdit } from "@fortawesome/free-solid-svg-icons";
    import Notification from "$lib/components/Notification.svelte";

    export let schools: School[] = [];
    export let message: string | undefined;
    export let isSuccess: boolean;
</script>

<div style="overflow: auto;">
    <table class="custom-table">
        <thead>
            <tr>
                <th class="has-text-black">School</th>
                <th class="has-text-black">Acronym</th>
                <th class="has-text-black">Address</th>
                <th class="has-text-black">Contact Number</th>
                <th class="has-text-black">Email</th>
                <th class="has-text-black">Action</th>
            </tr>
        </thead>
        <tbody>
            {#each schools as school}
            <tr>
                <td class="has-text-black">{school.schoolName}</td>
                <td class="has-text-black">{school.schoolAcronym}</td>
                <td class="has-text-black">{school.schoolAddress}</td>
                <td class="has-text-black">{school.schoolContactNum}</td>
                <td class="has-text-black">{school.schoolEmail}</td>
                <td class="has-text-centered has-text-black">
                    <a class="button is-link" href={`/school/edit/${school.id}`}>
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
    .custom-table {
        width: 100%;
        border-collapse: collapse;
    }

    .custom-table th,
    .custom-table td {
        border: 1px solid #ddd;
        padding: 8px;
    }

    .custom-table th {
        text-align: left;
        background-color: #f2f2f2;
    }

    .custom-table tbody tr:hover {
        background-color: lightgray;
        color: white;
    }
</style>
