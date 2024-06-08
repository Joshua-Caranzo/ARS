<script lang="ts">
	import { onMount } from "svelte";
	import type { CallResultDto } from "../../types/types";
	import type { ShiftsDto } from "./type";
	import Icon from "$lib/components/Icon.svelte";
	import { faEdit } from "@fortawesome/free-solid-svg-icons";
	import Notification from "$lib/components/Notification.svelte";

    export let shifts:ShiftsDto[] = [];
    export let message:string | undefined;
    export let isSuccess:boolean;

</script>


<div style="overflow: auto;">
    <table class="table is-fullwidth is-striped is-bordered">
        <thead>
          <tr>
            <th>Code</th>
            <th>Description</th>
            <th>Time Policy Code</th>
            <th>Type</th>
            <th>M Swipe</th>
            <th>Reg Hrs</th>
            <th>Mandatory Swipe</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
            {#each shifts as shift}
                <tr>
                    <td>{shift.shftCode}</td>
                    <td>{shift.shftDesc}</td>
                    <td>{shift.shftTpolicyCode}</td>
                    <td>{shift.shftType}</td>
                    <td>{shift.shftMswipe}</td>
                    <td>{shift.shftRegHrs}</td>
                    <td>{shift.shftMandatorySwipe}</td>
                    <td class="has-text-centered">
                        <a class="button is-link" href={`/shifts/edit/${shift.id}`}>
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