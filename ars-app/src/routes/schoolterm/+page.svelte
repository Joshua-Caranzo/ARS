<script lang="ts">
    import { onMount } from "svelte";
    import type { CallResultDto } from "../../types/types";
    import Icon from "$lib/components/Icon.svelte";
    import { faChevronLeft, faChevronRight, faPlus } from "@fortawesome/free-solid-svg-icons";
    import type { SchoolYear } from "./type";
    import { getSchoolYearList } from "./repo";
    import SchoolYearListTable from "./SchoolYearListTable.svelte";
	import IconButton from "$lib/components/IconButton.svelte";
	import Edit from "./edit/Edit.svelte";

    let errorMessage: string | undefined;
    let searchQuery: string = "";
    let currentPage: number = 1;
    let rowsPerPage: number = 10;
    let totalCount: number | null = 0;
    let gotoEdit: boolean = false;
    let s: SchoolYear;

    let schoolYearListCallResult: CallResultDto<SchoolYear[]> = {
        message: "",
        data: [],
        isSuccess: true,
        data2: [],
        totalCount:0
    };

    let schoolyears: SchoolYear[] = [];

    onMount(async () => {
        await fetchSchoolList();
    });

    async function fetchSchoolList() {
        try {
            console.log(schoolYearListCallResult)
            schoolYearListCallResult = await getSchoolYearList(searchQuery, currentPage, rowsPerPage);
            schoolyears = schoolYearListCallResult.data;
            totalCount = schoolYearListCallResult.totalCount;
            errorMessage = schoolYearListCallResult?.message || '';
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

    function goEdit(schoolYear: SchoolYear) {
        s = schoolYear;
        gotoEdit = true;
    }

    function handleClose() {
        gotoEdit = false;
        console.log(gotoEdit)
    }

</script>

{#if gotoEdit}
    <Edit {s} on:close={handleClose} ></Edit>
{:else}
    <h1 class="subtitle has-text-black">School Term List</h1>

    <div class="field is-flex">
        <div class="control" style="flex: 1;">
            <input class="input has-background-white has-text-black" type="text" placeholder="Search..." bind:value={searchQuery} on:input={handleSearch} />
        </div>

        <a class="button is-link mb-2 ml-4" href="/schoolterm/add">
            <Icon icon={faPlus} className="mr-2" />
            Add School Term
        </a>
    </div>

    <SchoolYearListTable schoolyears = {schoolyears} message={errorMessage} isSuccess={schoolYearListCallResult.isSuccess} {goEdit}/>

    <div class="pagination">
        <IconButton class="is-ghost" icon={faChevronLeft} on:click={() => changePage(currentPage - 1)} disabled={currentPage === 1} />
        {#if totalCount !== null}
            <span class="mx-4 has-text-black">{currentPage} / {Math.ceil(totalCount / rowsPerPage)}</span>
        {:else}
            <span>No data available.</span>
        {/if}
        <IconButton class="is-ghost" icon={faChevronRight} on:click={() => changePage(currentPage + 1)} disabled={totalCount === null || currentPage === Math.ceil(totalCount / rowsPerPage)} />
    </div>
{/if}

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
