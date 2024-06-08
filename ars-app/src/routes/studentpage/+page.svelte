<script lang="ts">
	import IconButton from "$lib/components/IconButton.svelte";
	import { faArrowUp, faUser } from "@fortawesome/free-solid-svg-icons";
	import { onMount } from "svelte";
	import { loggedInUser, shortcuts } from '$lib/store';
	import { getStudentById } from "./repo";
	import type { StudentFormData } from "./type";
	import Icon from "$lib/components/Icon.svelte";
	import type { CallResultDto } from "../../types/types";

	let studentinfo: StudentFormData | null = null; // Initialize as null
	let studentinfoCallResult: CallResultDto<StudentFormData>;
	let errorMessage: string | undefined;

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
			}
		} catch (error) {
			console.error('Error fetching student data:', error);
		}
	}
</script>

<svelte:head>
	<title>ARS System</title>
</svelte:head>

<div class="container">
	<div class="card mx-auto" style="background-color: lightgray;">
		<div class="card-content">
			<div id="print-section">
				<div class="columns is-centered">
					<div class="column is-narrow has-text-centered">
						<!-- User Icon -->
						<Icon icon={faUser}></Icon>
					</div>
				</div>
				{#if studentinfo}
                <div class="columns is-multiline">
                    <div class="column is-one-third">
                        <label class="label has-text-black">Name: {studentinfo.firstName} {studentinfo.middleName || ''} {studentinfo.lastName}</label>
                    </div>
             
                    <div class="column is-one-third">
                        <label class="label has-text-black">Id Number: {studentinfo.studentIdNumber}</label>
                    </div>
                    <div class="column is-one-third">
                        <label class="label has-text-black">Email: {studentinfo.email}</label>
                    </div>
                    <div class="column is-one-third">
                        <label class="label has-text-black">Contact Number: {studentinfo.contactNumber}</label>
                    </div>
                    <div class="column is-one-third">
                        <label class="label has-text-black">LRN: {studentinfo.lrn}</label>
                    </div>
                    <div class="column is-one-third">
                        <label class="label has-text-black">Last School Attended Year: {studentinfo.lastSchoolAttendedYear}</label>
                    </div>
              
                    <div class="column is-one-third">
                        <label class="label has-text-black">Birth Date: {new Date(studentinfo.birthdate).toLocaleDateString()}</label>
                    </div>
                    <div class="column is-one-third">
                        <label class="label has-text-black">Age: {studentinfo.age}</label>
                    </div>
                    <div class="column is-one-third">
                        <label class="label has-text-black">Birth Place: {studentinfo.birthplace}</label>
                    </div>
                    <div class="column is-one-third">
                        <label class="label has-text-black">Civil Status: {studentinfo.civilStatus}</label>
                    </div>
                    <div class="column is-one-third">
                        <label class="label has-text-black">Religion: {studentinfo.religion}</label>
                    </div>
                    <div class="column is-one-third">
                        <label class="label has-text-black">Mother Name: {studentinfo.mothersName || ''}</label>
                    </div>
                    <div class="column is-one-third">
                        <label class="label has-text-black">Mother Address: {studentinfo.mothersAddress || ''}</label>
                    </div>
                    <div class="column is-one-third">
                        <label class="label has-text-black">Father Name: {studentinfo.fathersName || ''}</label>
                    </div>
                    <div class="column is-one-third">
                        <label class="label has-text-black">Father Address: {studentinfo.fathersAddress || ''}</label>
                    </div>
                    <div class="column is-one-third">
                        <label class="label has-text-black">Guardian Name: {studentinfo.guardiansName}</label>
                    </div>
                    <div class="column is-one-third">
                        <label class="label has-text-black">Guardian Address: {studentinfo.guardiansAddress || ''}</label>
                    </div>
                    <div class="column is-one-third">
                        <label class="label has-text-black">Mother Contact Number: {studentinfo.mothersContactNumber || ''}</label>
                    </div>
                    <div class="column is-one-third">
                        <label class="label has-text-black">Father Contact Number: {studentinfo.fathersContactNumber || ''}</label>
                    </div>
                    <div class="column is-one-third">
                        <label class="label has-text-black">Guardian Contact Number: {studentinfo.guardiansContactNumber || ''}</label>
                    </div>
                    <div class="column is-one-third">
                        <label class="label has-text-black">Mother Email Address: {studentinfo.mothersEmailAddress || ''}</label>
                    </div>
                    <div class="column is-one-third">
                        <label class="label has-text-black">Father Email Address: {studentinfo.fathersEmailAddress || ''}</label>
                    </div>
                    <div class="column is-one-third">
                        <label class="label has-text-black">Guardian Email Address: {studentinfo.guardiansEmailAddress || ''}</label>
                    </div>
                    <div class="column is-one-third">
                        <label class="label has-text-black">Sex: {studentinfo.sex}</label>
                    </div>
                </div>
				{:else}
					<p>Loading student information...</p>
				{/if}
			</div>
		</div>
	</div>
</div>
