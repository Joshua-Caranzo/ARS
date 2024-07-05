<script lang="ts">
	import IconButton from "$lib/components/IconButton.svelte";
	import { faArrowUp, faUser } from "@fortawesome/free-solid-svg-icons";
	import { onMount } from "svelte";
	import { loggedInUser, shortcuts } from '$lib/store';
	import { getCurrentSchoolTerm, getStudentById } from "./repo";
	import type { Student } from "./type";
	import Icon from "$lib/components/Icon.svelte";
	import type { CallResultDto } from "../../types/types";

	let studentinfo: Student | null = null; // Initialize as null
	let studentinfoCallResult: CallResultDto<Student>;
	let errorMessage: string | undefined;
    let sy: number | null = null;

	onMount(async () => {
        const currentsyCallResult = await getCurrentSchoolTerm();
        if (currentsyCallResult.isSuccess) {
            sy = currentsyCallResult.data;
        } else {
            errorMessage = currentsyCallResult.message;
        }
        refresh();
	});

	// Function to fetch student data
    const refresh = async () => {
		try {
			if ($loggedInUser) {
				studentinfoCallResult = await getStudentById(parseInt($loggedInUser.uid), sy);
				studentinfo = studentinfoCallResult.data;
				errorMessage = studentinfoCallResult.message || ''; 
			}
		} catch (error) {
			console.error('Error fetching student data:', error);
		}
	}

    function formatBirthDay(date: Date, formatType: string = 'Date'): string {
		const d = new Date(date);
		if (formatType === 'Date') {
			const year = d.getFullYear();
			const month = String(d.getMonth() + 1).padStart(2, '0'); // Months are zero-indexed
			const day = String(d.getDate()).padStart(2, '0');
			return `${year}-${month}-${day}`;
		}
		return d.toISOString();
	}
</script>

<svelte:head>
	<title>ARS System</title>
</svelte:head>

<div class="container">
	<div class="card mx-auto" style="background-color: white;">
		<div class="card-content">
			<div id="print-section">
				<div class="columns is-centered">
					<div class="column is-narrow has-text-centered">
						<!-- User Icon -->
						<Icon icon={faUser}></Icon>
					</div>
				</div>
				{#if studentinfo}
                <div class="columns is-multiline has-text-black">
                    <div class="column is-one-third">
                        <label class="label has-text-black">Name</label>
                        <div>{studentinfo.firstName} {studentinfo.middleName || ''} {studentinfo.lastName}</div>
                    </div>
                    <div class="column is-one-third">
                        <label class="label has-text-black">Grade</label>
                        <div>{studentinfo.gradeLevel}</div>
                    </div>
                    <div class="column is-one-third">
                        <label class="label has-text-black">Id Number</label>
                        <div>{studentinfo.studentIdNumber}</div>
                    </div>
                    <div class="column is-one-third">
                        <label class="label has-text-black">Email</label>
                        <div>{studentinfo.email}</div>
                    </div>
                    <div class="column is-one-third">
                        <label class="label has-text-black">Contact Number</label>
                        <div>{studentinfo.contactNumber}</div>
                    </div>
                    <div class="column is-one-third">
                        <label class="label has-text-black">Student Address</label>
                        <div>{studentinfo.studentAddress || ''}</div>
                    </div>
                    <div class="column is-one-third">
                        <label class="label has-text-black">LRN</label>
                        <div>{studentinfo.lrn}</div>
                    </div>
                    <div class="column is-one-third">
                        <label class="label has-text-black">Last School Attended Year</label>
                        <div>{studentinfo.lastSchoolAttendedYear || ""}</div>
                    </div>
                    <div class="column is-one-third">
                        <label class="label has-text-black">Strand Name</label>
                        <div>{studentinfo.strandName}</div>
                    </div>
                    <div class="column is-one-third">
                        <label class="label has-text-black">Birth Date</label>
                        <div>{formatBirthDay(studentinfo.birthDate)}</div>
                    </div>
                    <div class="column is-one-third">
                        <label class="label has-text-black">Age</label>
                        <div>{studentinfo.age || ""}</div>
                    </div>
                    <div class="column is-one-third">
                        <label class="label has-text-black">Birth Place</label>
                        <div>{studentinfo.birthPlace}</div>
                    </div>
                    <div class="column is-one-third">
                        <label class="label has-text-black">Civil Status</label>
                        <div>{studentinfo.civilStatus}</div>
                    </div>
                    <div class="column is-one-third">
                        <label class="label has-text-black">Religion</label>
                        <div>{studentinfo.religion}</div>
                    </div>
                    <div class="column is-one-third">
                        <label class="label has-text-black">Mother Name</label>
                        <div>{studentinfo.motherName || ''}</div>
                    </div>
                    <div class="column is-one-third">
                        <label class="label has-text-black">Mother Address</label>
                        <div>{studentinfo.motherAddress || ''}</div>
                    </div>
                    <div class="column is-one-third">
                        <label class="label has-text-black">Father Name</label>
                        <div>{studentinfo.fatherName || ''}</div>
                    </div>
                    <div class="column is-one-third">
                        <label class="label has-text-black">Father Address</label>
                        <div>{studentinfo.fatherAddress || ''}</div>
                    </div>
                    <div class="column is-one-third">
                        <label class="label has-text-black">Guardian Name</label>
                        <div>{studentinfo.guardianName}</div>
                    </div>
                    <div class="column is-one-third">
                        <label class="label has-text-black">Guardian Address</label>
                        <div>{studentinfo.guardianAddress || ''}</div>
                    </div>
                    <div class="column is-one-third">
                        <label class="label has-text-black">Mother Contact Number</label>
                        <div>{studentinfo.motherContactNumber || ''}</div>
                    </div>
                    <div class="column is-one-third">
                        <label class="label has-text-black">Father Contact Number</label>
                        <div>{studentinfo.fatherContactNumber || ''}</div>
                    </div>
                    <div class="column is-one-third">
                        <label class="label has-text-black">Guardian Contact Number</label>
                        <div>{studentinfo.guardianContactNumber || ''}</div>
                    </div>
                    <div class="column is-one-third">
                        <label class="label has-text-black">Mother Email Address</label>
                        <div>{studentinfo.motherEmailAddress || ''}</div>
                    </div>
                    <div class="column is-one-third">
                        <label class="label has-text-black">Father Email Address</label>
                        <div>{studentinfo.fatherEmailAddress || ''}</div>
                    </div>
                    <div class="column is-one-third">
                        <label class="label has-text-black">Guardian Email Address</label>
                        <div>{studentinfo.guardianEmailAddress || ''}</div>
                    </div>
                    <div class="column is-one-third">
                        <label class="label has-text-black">Sex</label>
                        <div>{studentinfo.sex}</div>
                    </div>
                </div>
                
				{:else}
					<p>Please complete enrollment process to view your profile.</p>
				{/if}
			</div>
		</div>
	</div>
</div>
