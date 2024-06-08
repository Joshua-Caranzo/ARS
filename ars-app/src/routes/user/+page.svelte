<script lang="ts">
	import { onMount } from "svelte";
	import UserListTable from "./UserListTable.svelte";
	import type { CallResultDto } from "../../types/types";
	import type { UserDTO, UserTypeDTO } from "./type";
    import Icon from "$lib/components/Icon.svelte";
	import { faChevronCircleDown, faChevronLeft, faChevronRight, faPlus } from "@fortawesome/free-solid-svg-icons";
	import { getUserList, getUserType } from "./repo";
	import IconButton from "$lib/components/IconButton.svelte";

    let errorMessage: string | undefined;
    let searchQuery: string = "";
    let currentPage: number = 1;
    let rowsPerPage: number = 10;
    let totalCount: number | null = 0;

    let userListCallResult:CallResultDto<UserDTO[]> = {
        message:"",
        data: [],
        isSuccess:true,    
        data2:[],
        totalCount:0
    };

    let users:UserDTO[] = [];

    onMount(async () => {
        await fetchUserList();
    });


    async function fetchUserList() {
        try {
            userListCallResult= await getUserList(searchQuery, currentPage, rowsPerPage);
            users = userListCallResult.data;
            totalCount = userListCallResult.totalCount;
            errorMessage = userListCallResult?.message || '';
        } catch (err:any ) {
            errorMessage = err.message;
        }
    }

    function handleSearch() {
        currentPage = 1; // Reset to first page on new search
        fetchUserList();
    }

    function changePage(page: number) {
        currentPage = page;
        fetchUserList();
    }

    
</script>

<h1 class="subtitle has-text-black">User List</h1>
<div class="field is-flex">
    <div class="control" style="flex: 1;">
        <input class="input has-background-white has-text-black" type="text" placeholder="Search..." bind:value={searchQuery} on:input={handleSearch} />
    </div>

<a class="button is-link mb-2 ml-4" href="/user/add">
    <Icon icon={faPlus} className="mr-2"/>
    Add User
</a>
</div>
<UserListTable users={userListCallResult.data} message={errorMessage} isSuccess={userListCallResult.isSuccess}/>


<div class="pagination">
    <IconButton class="is-ghost" icon={faChevronLeft} on:click={() => changePage(currentPage - 1)} disabled={currentPage === 1} />
    {#if totalCount !== null}
        <span class="mx-4 has-text-black">{currentPage} / {Math.ceil(totalCount / rowsPerPage)}</span>
    {:else}
        <span>No data available.</span>
    {/if}
    <IconButton class="is-ghost" icon={faChevronRight} on:click={() => changePage(currentPage + 1)} disabled={totalCount === null || currentPage === Math.ceil(totalCount / rowsPerPage)} />
</div>

<style>
    .pagination button {
        margin: 0 5px;
        padding: 5px 10px;
        border: none;
        background-color: #f5f5f5;
        cursor: pointer;
    }

    .pagination button.active {
        background-color: #3273dc;
        color: white;
    }

    .pagination {
        margin-top: 20px;
        display: flex;
        justify-content: center;
    }
</style>
