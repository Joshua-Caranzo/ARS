<script lang="ts">
        import Notification from "$lib/components/Notification.svelte";
        import { loggedInUser, shortcuts } from '$lib/store';
	import type { Student } from "./type";
	import { getStudentList } from "./repo";
	import type { CallResultDto } from "../../types/types";
	import { onMount } from "svelte";
	import Icon from "$lib/components/Icon.svelte";
	import { faArrowUp, faChevronLeft, faChevronRight, faEdit, faPlus } from "@fortawesome/free-solid-svg-icons";
	import IconButton from "$lib/components/IconButton.svelte";
	import EnrollPopUp from "./EnrollPopUp.svelte";
	import EnrollPopUps from "../registrarenrolled/EnrollPopUp.svelte";


let errorMessage: string | undefined;
    let searchQuery: string = "";
    let currentPage: number = 1;
    let rowsPerPage: number = 10;
    let totalCount: number | null = 0;
    let isEnroll:boolean = false;
    let studentId:number = 0;
    let gradeId:number = 0;
    let strandId:number | null = null;
    let gradeLevel:string = "";
    let isMoveUp : boolean = false;
    let studentListCallResult:CallResultDto<Student[]> = {
        message:"",
        data: [],
        isSuccess:true,    
        data2:[],
        totalCount:0
    };

    let students:Student[] = [];

    onMount(async () => {
        await fetchStudentList();
        
    });

    function openEnroll(userId:number, gradeID:number, grade:string, strand:number|null)
    {
        isEnroll = !isEnroll;
        studentId = userId;
        strandId = strand;
        gradeId = gradeID;
     
        gradeLevel = grade;
    }

        function closeEnroll(){
            isEnroll =false;            
        }

        function closeMoveUp(){
            isMoveUp =false;            
        }

    async function fetchStudentList() {
        try {
          if( $loggedInUser){
          studentListCallResult= await getStudentList(searchQuery, currentPage, rowsPerPage, parseInt($loggedInUser.uid));
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

    function openMoveUp(userId:number, gradeID:number, grade:string, strand:number|null)
    {
        isMoveUp = true;
        studentId = userId;
        strandId = strand;
        gradeId = gradeID + 1;
     
        gradeLevel = grade;
    }

    </script>

    {#if isEnroll == true}
    <EnrollPopUp {studentId} gradeId={gradeId} gradeLevel = {gradeLevel} {strandId} on:close = {closeEnroll}></EnrollPopUp>
    {/if}

    {#if isMoveUp == true}
    <EnrollPopUps {studentId} gradeId={gradeId} gradeLevel = {gradeLevel} on:close = {closeMoveUp}></EnrollPopUps>
    {/if}
  <h1 class="subtitle has-text-black">Registered Students</h1>
  <div class="field is-flex">
      <div class="control" style="flex: 1;">
          <input class="input has-background-white has-text-black" type="text" placeholder="Search..." bind:value={searchQuery} on:input={handleSearch} />
      </div>

  </div>
  
<div style="overflow: auto;">
    <table class="table is-fullwidth is-bordered has-background-white my-custom-table">
        <thead>
          <tr>
            <th class="has-text-black">LRN</th>
            <th class="has-text-black">Name</th>
            <th class="has-text-black">Email</th>
            <th class="has-text-black">GradeLevel</th>
            <th class="has-text-black">Status</th>
            <th class="has-text-black">Actions</th>
          </tr>
        </thead>
        <tbody>
            {#if students}
            {#each students as user}
                <tr>
                    <td>{user.lrn}</td>
                    <td>{user.firstName} {user.middleName || ''} {user.lastName}</td>
                    <td>{user.email}</td>
                    <td>{user.gradeLevel}</td>
                    <td>{user.isEnrolled == true ? "For Move Up" : "For Enrollment"}</td>

                  
                    {#if user.isEnrolled == true}
                    <td class="has-text-centered">
                        <a class="button is-link"  on:click={() => openMoveUp(user.id, user.gradeLevelId, user.gradeLevel, user.strandId)}>
                            <Icon icon={faArrowUp} className="mr-2"/>
                            Move Up
                        </a>
                    </td>
                    {:else}
                    <td class="has-text-centered">
                        <a class="button is-link"  on:click={() => openEnroll(user.id, user.gradeLevelId, user.gradeLevel, user.strandId)}>
                            <Icon icon={faEdit} className="mr-2"/>
                            Enroll
                        </a>
                    </td>
                {/if}
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
    
  </style>
  
