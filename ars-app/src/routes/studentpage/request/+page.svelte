<script lang="ts">
	import IconButton from "$lib/components/IconButton.svelte";
	import { faArrowRight, faArrowUp, faUser } from "@fortawesome/free-solid-svg-icons";
	import { onMount } from "svelte";
	import { loggedInUser, shortcuts } from '$lib/store';
	import { getMoveUpStatus, getStudentById, requestToMoveUp } from "../repo";
	import type { StudentFormData } from "../type";
	import Icon from "$lib/components/Icon.svelte";
	import type { CallResultDto } from "../../../types/types";
	import { getGradeLevels, getSchool } from "../../repo";

	let studentinfo: StudentFormData | null = null; // Initialize as null
	let studentinfoCallResult: CallResultDto<StudentFormData>;
	let errorMessage: string | undefined;
    let gradeLevel: string;
    let nextGradeLevel:string;
    let moveUp:boolean;
    let moveUpMessage : string |undefined;

	onMount(async () => {
		await fetchStudentData();
	});

	// Function to fetch student data
	async function fetchStudentData() {
		try {
			if ($loggedInUser) {
				studentinfoCallResult = await getStudentById(parseInt($loggedInUser.uid), true);
				studentinfo = studentinfoCallResult.data;
				console.log(studentinfo);
				errorMessage = studentinfoCallResult.message || '';
                const glListCallResult = await getGradeLevels();
                const gradeLevlels = glListCallResult.data;
                const g = gradeLevlels.find(gl => gl.id === studentinfo?.gradeLevelId) || null;
                gradeLevel = g?.level ?? "";
                const nextID = studentinfo.gradeLevelId + 1;
                const gn = gradeLevlels.find(gl => gl.id === nextID) || null;
                nextGradeLevel = gn?.level ?? "";    
                
                const MoveUpStatusCall = await getMoveUpStatus(studentinfo.id);
                moveUp = MoveUpStatusCall.data;
                moveUpMessage = MoveUpStatusCall.message;
			}
		} catch (error) {
			console.error('Error fetching student data:', error);
		}
	}

    function close()
    {
        setTimeout(() => {
                    window.location.href = "/studentpage";
                });
    }
</script>

<svelte:head>
	<title>ARS System</title>
</svelte:head>


<div class="modal is-active">
    <div class="modal-background"></div>
    <div class="modal-content ">
      <div class="box has-background-white">
        <h2 class="title is-4 has-text-black">{gradeLevel} <Icon icon={faArrowRight} className="mt-2"></Icon> {nextGradeLevel}</h2>
        
        <div class="field">
          <div class="control">
            <label class="has-text-black">{moveUpMessage}</label>
          </div>
        </div>

        <div class="field">
            <div class="buttons is-right">
              <IconButton class="button is-link" disabled = {moveUp == false} on:click={() => requestToMoveUp(studentinfo?.id ?? 0)}>Send Request</IconButton>
            </div>
          </div>
      </div>
    </div>
    <button class="modal-close is-large" aria-label="close" on:click={close}></button>
  </div>
  <style>
    .modal {
      display: flex !important;
      align-items: center;
      justify-content: center;
    }
  </style>