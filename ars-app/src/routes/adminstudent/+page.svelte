<script lang="ts">
    import Notification from "$lib/components/Notification.svelte";
    import { loggedInUser, shortcuts } from '$lib/store';
    import type { Student, EnrollmentHistory, SchoolYear } from "./type";
    import { getStudentList, getStudentById, getSchoolYearList, getCurrentSchoolTerm, getNotes, unlockUser } from "./repo";
    import type { CallResultDto } from "../../types/types";
    import { onMount } from "svelte";
    import { faChevronLeft, faChevronRight, faEdit, faLockOpen } from "@fortawesome/free-solid-svg-icons";
    import IconButton from "$lib/components/IconButton.svelte";

    let errorMessage: string | undefined;
    let searchQuery: string = "";
    let currentPage: number = 1;
    let rowsPerPage: number = 10;
    let totalCount: number | null = 0;
    let schoolYears: SchoolYear[] = [];
    let selectedRow: number | null = null;

    let syListCallResult: CallResultDto<SchoolYear[]> = {
        message: "",
        data: [],
        isSuccess: true,    
        data2: [],
        totalCount: 0
    };

    let studentListCallResult: CallResultDto<Student[]> = {
        message: "",
        data: [],
        isSuccess: true,    
        data2: [],
        totalCount: 0
    };

    let students: Student[] = [];
    let sy: number | null = null;


    onMount(() => {
        initializeCurrentSchoolYear();
    });

    async function initializeCurrentSchoolYear() {
        const currentsyCallResult = await getCurrentSchoolTerm();
        if (currentsyCallResult.isSuccess) {
            sy = currentsyCallResult.data;
        } else {
            errorMessage = currentsyCallResult.message;
        }
        fetchStudentList();
    }

    async function fetchStudentList() {
        try {
            if ($loggedInUser && sy !=null) {
                studentListCallResult = await getStudentList(searchQuery, currentPage, rowsPerPage, parseInt($loggedInUser.uid), sy);
                syListCallResult = await getSchoolYearList();
                schoolYears = syListCallResult.data;
                students = studentListCallResult.data;
                totalCount = studentListCallResult.totalCount;
                errorMessage = studentListCallResult?.message || '';
            }
        } catch (err: any) {
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

    async function unlock(studentId:number)
    {
        await unlockUser(studentId);
        fetchStudentList();
    }
</script>


         <h1 class="subtitle has-text-black">Student List for Current School Term
         </h1>
            <div class="field is-flex">
                <div class="control" style="flex: 1;">
                     <input class="input has-background-white has-text-black" type="text" placeholder="Search..." bind:value={searchQuery} on:input={handleSearch} />
                </div>

            </div>

<div style="overflow: auto;">
<table class="table  is-bordered is-fullwidth has-background-white my-custom-table">
    <thead>
      <tr>
        <th class="has-text-black">Name</th>
        <th class="has-text-black">GradeLevel</th>
        <th class="has-text-black">Action</th>
      </tr>
    </thead>
    <tbody>
        {#each students as user}
            <tr>
             
                <td>{user.firstName} {user.middleName || ''} {user.lastName}</td>
                
                <td>{user.gradeLevelForSy}</td>
                <td class="has-text-centered">
                    {#if user.isLockedOut == true}
                    <IconButton class="button-blue has-text-white" icon={faLockOpen} on:click={() => unlock(user.id)}>Unlock User Account</IconButton>
                    {:else}
                        No Action Needed
                    {/if}
                </td>
            </tr>
        {/each}    
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
    .icon.is-large {
        font-size: 3rem;
    }
    .mt-2 {
        margin-top: 1rem;
    }
    .mt-5 {
        margin-top: 2rem;
    }

    .selected {
        background-color: #f0f0f0; /* Or any color you want for the selected row */
    }
  </style>
  
