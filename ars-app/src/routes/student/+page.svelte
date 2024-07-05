<script lang="ts">
    import Notification from "$lib/components/Notification.svelte";
    import { loggedInUser, shortcuts } from '$lib/store';
import type { StudentAdminDto } from "./type";
import { getStudentList } from "./repo";
import type { CallResultDto } from "../../types/types";
import { onMount } from "svelte";
import Icon from "$lib/components/Icon.svelte";
import { faArrowUp, faBoxOpen, faChevronLeft, faChevronRight, faEye } from "@fortawesome/free-solid-svg-icons";
import IconButton from "$lib/components/IconButton.svelte";
import EnrollPopUps from "../registrarenrolled/EnrollPopUp.svelte";
	import ViewStudent from "./ViewStudent.svelte";
	import type { StudentFormData } from "../type";


let errorMessage: string | undefined;
let searchQuery: string = "";
let currentPage: number = 1;
let rowsPerPage: number = 10;
let totalCount: number | null = 0;
let isView:boolean = false;
let studentId:number = 0;
let studentListCallResult:CallResultDto<StudentAdminDto[]> = {
    message:"",
    data: [],
    isSuccess:true,    
    data2:[],
    totalCount:0
};

let students:StudentAdminDto[] = [];

onMount(async () => {
    await fetchStudentList();
    
});

    function closeView(){
        isView =false;            
    }

async function fetchStudentList() {
    try {
      if( $loggedInUser){
      studentListCallResult= await getStudentList(searchQuery, currentPage, rowsPerPage);
        students = studentListCallResult.data;
        totalCount = studentListCallResult.totalCount;
        errorMessage = studentListCallResult?.message || '';
      }
    } catch (err:any ) {
        errorMessage = err.message;
    }
}

function handleSearch() {
    currentPage = 1; // Reset to first page on new search
    fetchStudentList();
}

function changePage(page: number) {
    currentPage = page;
    fetchStudentList();
}

function reloadPage() {
    window.location.reload();
}

function openView(userId:number)
{
    isView = true;
    studentId = userId;
}

</script>

{#if isView == true}
    <ViewStudent {studentId} on:close = {closeView}></ViewStudent>
{/if}
<h1 class="subtitle has-text-black">Students List</h1>
<div class="field is-flex">
  <div class="control" style="flex: 1;">
      <input class="input has-background-white has-text-black" type="text" placeholder="Search..." bind:value={searchQuery} on:input={handleSearch} />
  </div>

</div>

<div style="overflow: auto;">
<table class="table is-fullwidth is-bordered has-background-white my-custom-table">
    <thead>
      <tr>
        <th class="has-text-black">Id Number</th>
        <th class="has-text-black">Name</th>
        <th class="has-text-black">School</th>
        <th class="has-text-black">Action</th>
      </tr>
    </thead>
    <tbody>
        {#if students}
        {#each students as user}
            <tr>
                <td>{user.studentIdNumber}</td>
                <td>{user.lastName}, {user.firstName} {user.middleName || ''} {user.suffix || ""}</td>
                <td>{user.schoolName}</td>
                <td class="has-text-centered"> <a class="button button-blue"  on:click={() => openView(user.id)}>
                    <Icon icon={faEye} className="mr-2"/>
                    View Student Details
                </a></td>
            </tr>
        {/each}   
        {/if} 
    </tbody>
</table>
</div>

{#if errorMessage != ''}
<Notification errorMessage={errorMessage}></Notification>
{/if}

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

