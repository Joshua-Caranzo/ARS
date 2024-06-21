<script lang="ts">
	import { onMount } from "svelte";
	import type { CallResultDto } from "../../types/types";
    import Icon from "$lib/components/Icon.svelte";
	import { faChevronCircleDown, faChevronLeft, faChevronRight, faPlus } from "@fortawesome/free-solid-svg-icons";
	import IconButton from "$lib/components/IconButton.svelte";
    import { loggedInUser, shortcuts } from '$lib/store';
	import SectionListTable from "./SectionListTable.svelte";
	import type { SchoolSectionDto } from "./type";
	import { getSectionList } from "./repo";
	import Edit from "./edit/Edit.svelte";

    let errorMessage: string | undefined;
    let searchQuery: string = "";
    let currentPage: number = 1;
    let rowsPerPage: number = 10;
    let totalCount: number | null = 0;
    let gotoEdit: boolean = false;
    let s: SchoolSectionDto;

    let sectionListCallResult:CallResultDto<SchoolSectionDto[]> = {
        message:"",
        data: [],
        isSuccess:true,    
        data2:[],
        totalCount:0
    };

    let sections:SchoolSectionDto[] = [];

    onMount(async () => {
        await fetchSectionList();
    });


    async function fetchSectionList() {
        try {
            if($loggedInUser){
                sectionListCallResult= await getSectionList(searchQuery, currentPage, rowsPerPage, parseInt($loggedInUser?.uid));
            sections = sectionListCallResult.data;
            totalCount = sectionListCallResult.totalCount;
            errorMessage = sectionListCallResult?.message || '';
            }
        } catch (err:any ) {
            errorMessage = err.message;
        }
    }

    function handleSearch() {
        currentPage = 1; // Reset to first page on new search
        fetchSectionList();
    }

    function changePage(page: number) {
        currentPage = page;
        fetchSectionList();
    }

    function goEdit(user: SchoolSectionDto) {
        s = user;
        gotoEdit = true;
    }

    function handleClose() {
        gotoEdit = false;
        console.log(gotoEdit)
    }

    $: if(!gotoEdit){
        (async () => await fetchSectionList())();
    }

</script>
{#if gotoEdit}
    <Edit {s} on:close={handleClose} ></Edit>
{:else}
    <h1 class="subtitle has-text-black">Section List</h1>
    <div class="field is-flex">
        <div class="control" style="flex: 1;">
            <input class="input has-background-white has-text-black" type="text" placeholder="Search..." bind:value={searchQuery} on:input={handleSearch} />
        </div>
        <a class="button is-link mb-2 ml-4" href="/adminsection/add">
            <Icon icon={faPlus} className="mr-2"/>
            Add Section
        </a>
    </div>
    <SectionListTable sections={sectionListCallResult.data} message={errorMessage} isSuccess={sectionListCallResult.isSuccess} {goEdit}/>

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
