<script lang="ts">
    import { onMount } from "svelte";
    import type { CallResultDto } from "../../types/types";
    import Icon from "$lib/components/Icon.svelte";
    import { faChevronLeft, faChevronRight, faPlus } from "@fortawesome/free-solid-svg-icons";
    import type { School } from "./type";
    import { getSchoolList } from "./repo";
    import SchoolListTable from "./SchoolListTable.svelte";
	import IconButton from "$lib/components/IconButton.svelte";

    let errorMessage: string | undefined;
    let searchQuery: string = "";
    let currentPage: number = 1;
    let rowsPerPage: number = 10;
    let totalCount: number | null = 0;

    let schoolListCallResult: CallResultDto<School[]> = {
        message: "",
        data: [],
        isSuccess: true,
        data2: [],
        totalCount:0
    };

    let schools: School[] = [];

    onMount(async () => {
        await fetchSchoolList();
    });

    async function fetchSchoolList() {
        try {
            console.log(schoolListCallResult)
            schoolListCallResult = await getSchoolList(searchQuery, currentPage, rowsPerPage);
            schools = schoolListCallResult.data;
            totalCount = schoolListCallResult.totalCount;
            errorMessage = schoolListCallResult?.message || '';
        } catch (err: any) {
            errorMessage = err.message;
        }
    }

    function handleSearch() {
        currentPage = 1; // Reset to first page on new search
        fetchSchoolList();
    }

    function changePage(page: number) {
        currentPage = page;
        fetchSchoolList();
    }
</script>

<h1 class="subtitle has-text-black">School List</h1>

<div class="field is-flex">
    <div class="control" style="flex: 1;">
        <input class="input has-background-white has-text-black" type="text" placeholder="Search..." bind:value={searchQuery} on:input={handleSearch} />
    </div>

    <a class="button is-link mb-2 ml-4" href="/school/add">
        <Icon icon={faPlus} className="mr-2" />
        Add School
    </a>
</div>

<SchoolListTable schools={schools} message={errorMessage} isSuccess={schoolListCallResult.isSuccess} />

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
