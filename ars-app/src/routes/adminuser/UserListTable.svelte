<script lang="ts">
    import type { UserDTO } from "./type";
    import { faEdit, faLockOpen } from "@fortawesome/free-solid-svg-icons";
    import Notification from "$lib/components/Notification.svelte";
    import IconButton from "$lib/components/IconButton.svelte";
	import { unlockUser } from "./repo";
	import { createEventDispatcher } from "svelte";

    export let users: UserDTO[] = [];
    export let message: string | undefined;
    export let isSuccess: boolean;
    export let goEdit: (user: UserDTO) => void; // Add this line

    const dispatch = createEventDispatcher();
    function refresh() {
    dispatch('refresh');
  }
  async function unlock(userId:number)
  {
    await unlockUser(userId);
    refresh();
  }
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
                    <td>{user.userTypeName}</td>
                    <td class="has-text-centered">
                        <IconButton class="button-blue has-text-white" icon={faEdit} on:click={() => goEdit(user)} label="Edit"/>
                            {#if user.isLockedOut == true}
                            <IconButton class="button-blue has-text-white" icon={faLockOpen} on:click = {() => unlock(user.id)} label="Unlock Account"/>
                            {/if}
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
    .button-blue
	{
		background-color: #063F78;
        color:white;
	}

</style>
