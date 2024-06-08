<script lang="ts">
	import type { UserDTO } from "./type";
	import Icon from "$lib/components/Icon.svelte";
	import { faEdit } from "@fortawesome/free-solid-svg-icons";
	import Notification from "$lib/components/Notification.svelte";

    export let users:UserDTO[] = [];
    export let message:string | undefined;
    export let isSuccess:boolean;

</script>

<div style="overflow: auto;">
    <table class="table is-fullwidth is-bordered has-background-white my-custom-table">
        <thead>
          <tr>
            <th>Username</th>
            <th>First Name</th>
            <th>Last Name</th>
            <th>Email</th>
            <th>User Type</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
            {#each users as user}
                <tr>
                    <td>{user.userName}</td>
                    <td>{user.firstName}</td>
                    <td>{user.lastName}</td>
                    <td>{user.email}</td>
                    <td>
                        {user.userTypeName}
                    </td>
                    <td class="has-text-centered">
                        <a class="button is-link" href={`/adminuser/edit/${user.id}`}>
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