<script lang="ts">
    import type { SchoolYear} from "./type";
    import Icon from "$lib/components/Icon.svelte";
    import { faEdit } from "@fortawesome/free-solid-svg-icons";
    import Notification from "$lib/components/Notification.svelte";
	import IconButton from "$lib/components/IconButton.svelte";

    export let schoolyears: SchoolYear[] = [];
    export let message: string | undefined;
    export let isSuccess: boolean;
    export let goEdit: (user: SchoolYear) => void;
</script>

<div style="overflow: auto;">
    <table class="custom-table">
        <thead>
            <tr>
                <th class="has-text-black">From</th>
                <th class="has-text-black">To</th>
                <th class="has-text-black">Active</th>
                <th class="has-text-black">Action</th>
            </tr>
        </thead>
        <tbody>
            {#each schoolyears as school}
            <tr>
                <td class="has-text-black">{school.fromSchoolTerm}</td>
                <td class="has-text-black">{school.toSchoolTerm}</td>
                <td class="has-text-black">{school.isActive}</td>
                <td class="has-text-centered has-text-black">
                    <IconButton icon={faEdit} on:click={() => goEdit(school)} label="Edit"/>
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
