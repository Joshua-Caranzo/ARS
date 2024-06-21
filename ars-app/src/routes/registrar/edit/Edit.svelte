<script lang="ts">
	import type { CallResultDto } from '../../../types/types';
	import Icon from '$lib/components/Icon.svelte';
	import { faArrowLeft, faInfoCircle } from '@fortawesome/free-solid-svg-icons';
	import { createEventDispatcher, onMount } from 'svelte';
	import Notification from '$lib/components/Notification.svelte';
	import type { GradeLevel, StrandDto, StudentFormData } from '../../type';
	import { editStudent, getGradeLevels, getStrands } from '../../repo';
	import { loggedInUser, shortcuts } from '$lib/store';
	import { getStudentById } from '../repo';

	const dispatch = createEventDispatcher();
	export let s: StudentFormData;

	let result: CallResultDto<StudentFormData>;
	let student: StudentFormData = {
		id: 0,
		lastName: '',
		firstName: '',
		middleName: '',
		suffix: '',
		email: '',
		contactNumber: '',
		lrn: '',
		birthdate: new Date(),
		birthplace: '',
		civilStatus: '',
		religion: '',
		sex: '',
		mothersName: '',
		mothersAddress: '',
		mothersContactNumber: null,
		mothersEmailAddress: null,
		fathersName: '',
		fathersAddress: '',
		fathersContactNumber: null,
		fathersEmailAddress: null,
		guardiansName: '',
		guardiansAddress: '',
		guardiansContactNumber: null,
		guardiansEmailAddress: null,
		lastSchoolAttended: '',
		lastSchoolAttendedYear: '',
		gradeLevelId: 0,
		strandId: null,
		isMotherDeceased: false,
		isFatherDeceased: false,
		guardianRelationship: '',
		motherOccupation: null,
		fatherOccupation: null
	};

	let errorMessage: string | undefined;
	let successMessage: string | undefined;
	let schoolYearFrom: string;
	let schoolYearTo: string;
	let birthdate: string;

	let gradeLevelListCallResult: CallResultDto<GradeLevel[]> = {
		message: '',
		data: [],
		isSuccess: true,
		data2: [],
		totalCount: null
	};
	let gradeLevels: GradeLevel[] = [];
	let strandListCallResult: CallResultDto<StrandDto[]> = {
		message: '',
		data: [],
		isSuccess: true,
		data2: [],
		totalCount: null
	};
	let strands: StrandDto[] = [];
	let gradeLevel: number;
	let strandID: number | null = null;
	let sameFather: boolean;
	let sameMother: boolean;

	onMount(async () => {
		try {
			const result = await getStudentById(s.id, false);

			if (result.isSuccess) {
				student = {
					...result.data,
					id: s.id
				};
				birthdate = formatBirthDay(new Date(student.birthdate), 'Date');
				gradeLevel = student.gradeLevelId;

				strandID = student.strandId;
				if (student.lastSchoolAttendedYear)
					[schoolYearFrom, schoolYearTo] = student.lastSchoolAttendedYear.split('-');
				gradeLevelListCallResult = await getGradeLevels();
				gradeLevels = gradeLevelListCallResult.data;
				errorMessage = gradeLevelListCallResult?.message || '';
				strandListCallResult = await getStrands();
				strands = strandListCallResult.data;
			} else {
				errorMessage = result.message;
			}
		} catch (err: any) {
			errorMessage = err.message;
		}
	});

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

	async function handleSubmit(e: Event) {
		e.preventDefault();
		student = { ...student, id: s.id };
		student.birthdate = new Date(birthdate);
		student.gradeLevelId = gradeLevel;
		student.strandId = strandID;
		try {
			if ($loggedInUser) {
				const callResult: CallResultDto<object> = await editStudent(
					student,
					parseInt($loggedInUser.uid)
				);

				if (callResult.isSuccess) {
					successMessage = callResult.message;
					setTimeout(() => {
						window.location.href = '/registrar';
					}, 2000);
				} else {
					errorMessage = callResult.message;
				}
			}
		} catch (error: any) {
			errorMessage = error.message;
		}
	}
	function fillGuardianWithFather(s: StudentFormData) {
		if (sameFather == true) {
			student.guardiansName = s.fathersName ?? '';
			student.guardiansAddress = s.fathersAddress ?? '';
			student.guardiansContactNumber = s.fathersContactNumber ?? '';
			student.guardiansEmailAddress = s.fathersEmailAddress ?? '';
			student.guardianRelationship = 'Father';
		} else {
			student.guardiansName = '';
			student.guardiansAddress = '';
			student.guardiansContactNumber = '';
			student.guardiansEmailAddress = '';
			student.guardianRelationship = '';
		}
	}

	function fillGuardianWithMother(s: StudentFormData) {
		if (sameMother == true) {
			student.guardiansName = s.mothersName ?? '';
			student.guardiansAddress = s.mothersAddress ?? '';
			student.guardiansContactNumber = s.mothersContactNumber ?? '';
			student.guardianRelationship = 'Mother';
			student.guardiansEmailAddress = s.mothersEmailAddress ?? '';
		} else {
			student.guardiansName = '';
			student.guardiansAddress = '';
			student.guardiansContactNumber = '';
			student.guardiansEmailAddress = '';
			student.guardianRelationship = '';
		}
	}

	function handleClose() {
		dispatch('close');
	}
</script>

<div class="container mt-6">
	<div class="is-flex is-align-items-center">
		<button class="button is-link" on:click={handleClose}>
			<Icon icon={faArrowLeft} />
		</button>
		<h1 class="subtitle ml-2 has-text-black">Edit Student</h1>
	</div>
	<section class="section">
		<div class="container">
			<div class="card mx-auto" style="background-color: lightgray;">
				<div class="card-content">
					<form class="mx-auto" on:submit={handleSubmit}>
						<fieldset>
							<div class="columns mt-4">
								<div class="column has-background-light is-flex">
									<Icon className="mr-2 mt-1" icon={faInfoCircle}></Icon>
									<h2 class="title is-4 has-text-black">Student Information</h2>
								</div>
							</div>
							<div class="columns is-multiline">
								<!-- Row 1 -->
								<div class="column is-one-third">
									<div class="field mb-4">
										<div class="control">
											<div class="is-flex">
												<label class="label has-text-black">Last Name</label>
												<label class="label has-text-danger"> *</label>
											</div>
											<input
												class="input has-background-white has-text-black"
												type="text"
												bind:value={student.lastName}
												required
											/>
										</div>
									</div>
								</div>
								<div class="column is-one-third">
									<div class="field mb-4">
										<div class="control">
											<div class="is-flex">
												<label class="label has-text-black">First Name</label>
												<label class="label has-text-danger"> *</label>
											</div>
											<input
												class="input has-background-white has-text-black"
												type="text"
												bind:value={student.firstName}
												required
											/>
										</div>
									</div>
								</div>
								<div class="column is-one-quarter">
									<div class="field mb-4">
										<div class="control">
											<label class="label has-text-black">Middle Name</label>
											<input
												class="input has-background-white has-text-black"
												type="text"
												bind:value={student.middleName}
											/>
										</div>
									</div>
								</div>

								<div class="column auto">
									<div class="field mb-4">
										<div class="control">
											<label class="label has-text-black">Suffix</label>
											<input
												class="input has-background-white has-text-black"
												type="text"
												bind:value={student.suffix}
											/>
										</div>
									</div>
								</div>

								<!-- Row 2 -->
								<div class="column is-one-quarter">
									<div class="field mb-4">
										<div class="control">
											<div class="is-flex">
												<label class="label has-text-black">Email</label>
												<label class="label has-text-danger"> *</label>
											</div>
											<input
												class="input has-background-white has-text-black"
												type="email"
												bind:value={student.email}
												required
											/>
										</div>
									</div>
								</div>
								<div class="column is-one-quarter">
									<div class="field mb-4">
										<div class="control">
											<div class="is-flex">
												<label class="label has-text-black">Contact Number</label>
												<label class="label has-text-danger"> *</label>
											</div>
											<input
												class="input has-background-white has-text-black"
												type="text"
												bind:value={student.contactNumber}
											/>
										</div>
									</div>
								</div>
								<div class="column is-one-quarter">
									<div class="field">
										<div class="control">
											<div class="is-flex">
												<label class="label has-text-black">Grade Level</label>
												<label class="label has-text-danger"> *</label>
											</div>
											<select
												class="input has-background-white has-text-black"
												bind:value={gradeLevel}
											>
												<option value={0}>-- SELECT --</option>
												{#each gradeLevels as gradeLevel}
													<option value={gradeLevel.id}>{gradeLevel.level}</option>
												{/each}
											</select>
										</div>
									</div>
								</div>
								<div class="column is-one-quarter">
									<div class="field">
										<div class="control">
											<label class="label has-text-black">Strand</label>
											<select
												class="input has-background-white has-text-black"
												disabled={student.gradeLevelId <= 13}
												bind:value={strandID}
											>
												<option value={null}>-- SELECT --</option>
												{#each strands as strand}
													<option value={strand.id}>
														{strand.strandAbbrev ? strand.strandAbbrev : strand.strandName}</option
													>
												{/each}
											</select>
										</div>
									</div>
								</div>
								<!-- Row 3 -->
								<div class="column is-one-quarter">
									<div class="field mb-4">
										<div class="control">
											<div class="is-flex">
												<label class="label has-text-black">Birthdate</label>
												<label class="label has-text-danger"> *</label>
											</div>
											<input
												class="input has-background-white has-text-black"
												type="date"
												bind:value={birthdate}
												required
											/>
										</div>
									</div>
								</div>
								<div class="column is-one-quarter">
									<div class="field mb-4">
										<div class="control">
											<div class="is-flex">
												<label class="label has-text-black">Civil Status</label>
												<label class="label has-text-danger"> *</label>
											</div>
											<div class="select">
												<select
													class="has-background-white has-text-black"
													bind:value={student.civilStatus}
													required
												>
													<option value="" disabled>Select Civil Status</option>
													<option value="Single">Single</option>
													<option value="Married">Married</option>
												</select>
											</div>
										</div>
									</div>
								</div>
								<div class="column is-one-quarter">
									<div class="field mb-4">
										<div class="control">
											<div class="is-flex">
												<label class="label has-text-black">Birth Place</label>
												<label class="label has-text-danger"> *</label>
											</div>
											<input
												class="input has-background-white has-text-black"
												type="text"
												bind:value={student.birthplace}
												required
											/>
										</div>
									</div>
								</div>
								<div class="column is-one-quarter">
									<div class="field mb-4">
										<div class="control">
											<label class="label has-text-black">LRN</label>
											<input
												class="input has-background-white has-text-black"
												disabled={student.gradeLevelId <= 3}
												type="text"
												bind:value={student.lrn}
											/>
										</div>
									</div>
								</div>

								<!-- Row 4 -->
								<div class="column is-one-quarter">
									<div class="field mb-4">
										<div class="control">
											<div class="is-flex">
												<label class="label has-text-black">Religion</label>
												<label class="label has-text-danger"> *</label>
											</div>
											<input
												class="input has-background-white has-text-black"
												type="text"
												bind:value={student.religion}
												required
											/>
										</div>
									</div>
								</div>
								<div class="column is-one-quarter">
									<div class="field mb-4">
										<div class="is-flex">
											<label class="label has-text-black">Sex</label>
											<label class="label has-text-danger"> *</label>
										</div>
										<div class="select">
											<select
												class="has-background-white has-text-black"
												bind:value={student.sex}
												required
											>
												<option value="" disabled>Select Sex</option>
												<option value="Male">Male</option>
												<option value="Female">Female</option>
											</select>
										</div>
									</div>
								</div>

								<div class="column is-one-quarter">
									<div class="field mb-4">
										<div class="control">
											<label class="label has-text-black">Last School Attended</label>
											<input
												class="input has-background-white has-text-black"
												type="text"
												bind:value={student.lastSchoolAttended}
												required
											/>
										</div>
									</div>
								</div>
								<div class="column is-one-quarter">
									<div class="field mb-4">
										<div class="control">
											<label class="label has-text-black">Last School Year Attended</label>
											<div class="columns">
												<div class="column is-half">
													<input
														id="fromSchoolYear"
														class="input has-background-white has-text-black"
														type="number"
														min="2000"
														max="2100"
														placeholder="From (YYYY)"
														bind:value={schoolYearFrom}
														required
													/>
												</div>
												<div class="column is-half">
													<input
														id="toSchoolYear"
														class="input has-background-white has-text-black"
														type="number"
														min="2010"
														max="2100"
														placeholder="To (YYYY)"
														bind:value={schoolYearTo}
														required
													/>
												</div>
											</div>
										</div>
									</div>
								</div>

								<div class="column is-full has-background-light is-flex">
									<Icon className="mr-2 mt-1" icon={faInfoCircle}></Icon>
									<h2 class="title is-4 has-text-black">Parents Information</h2>
								</div>
								<div class="column is-half">
									<div class="field mb-4">
										<div class="control">
											<label class="label has-text-black">Mother's Name</label>
											<input
												class="input has-background-white has-text-black"
												type="text"
												bind:value={student.mothersName}
											/>
										</div>
									</div>
								</div>
								<div class="column is-half">
									<div class="field mb-4">
										<div class="control">
											<label class="label has-text-black">Mother's Address</label>
											<input
												class="input has-background-white has-text-black"
												type="text"
												bind:value={student.mothersAddress}
											/>
										</div>
									</div>
								</div>
								<div class="column is-one-quarter">
									<div class="field mb-4">
										<div class="control">
											<label class="label has-text-black">Mother's Contact</label>
											<input
												class="input has-background-white has-text-black"
												type="text"
												bind:value={student.mothersContactNumber}
											/>
										</div>
									</div>
								</div>
								<div class="column is-one-quarter">
									<div class="field mb-4">
										<div class="control">
											<label class="label has-text-black">Mother's Occupation</label>
											<input
												class="input has-background-white has-text-black"
												type="text"
												bind:value={student.motherOccupation}
											/>
										</div>
									</div>
								</div>
								<div class="column is-one-quarter">
									<div class="field mb-4">
										<div class="control">
											<label class="label has-text-black">Mother's Email</label>
											<input
												class="input has-background-white has-text-black"
												type="text"
												bind:value={student.mothersEmailAddress}
											/>
										</div>
									</div>
								</div>
								<div class="column auto">
									<div class="field mb-4 mt-4">
										<div class="is-flex mt-6">
											<div class="control">
												<input
													class="checkbox"
													type="checkbox"
													bind:checked={student.isMotherDeceased}
												/>
											</div>
											<label class="label has-text-black ml-2"> Deceased?</label>
										</div>
									</div>
								</div>
								<div class="column is-half">
									<div class="field mb-4">
										<div class="control">
											<label class="label has-text-black">Father's Name</label>
											<input
												class="input has-background-white has-text-black"
												type="text"
												bind:value={student.fathersName}
											/>
										</div>
									</div>
								</div>
								<div class="column is-half">
									<div class="field mb-4">
										<div class="control">
											<label class="label has-text-black">Father's Address</label>
											<input
												class="input has-background-white has-text-black"
												type="text"
												bind:value={student.fathersAddress}
											/>
										</div>
									</div>
								</div>
								<div class="column is-one-quarter">
									<div class="field mb-4">
										<div class="control">
											<label class="label has-text-black">Father's Contact</label>
											<input
												class="input has-background-white has-text-black"
												type="text"
												bind:value={student.fathersContactNumber}
											/>
										</div>
									</div>
								</div>
								<div class="column is-one-quarter">
									<div class="field mb-4">
										<div class="control">
											<label class="label has-text-black">Father's Occupation</label>
											<input
												class="input has-background-white has-text-black"
												type="text"
												bind:value={student.fatherOccupation}
											/>
										</div>
									</div>
								</div>
								<div class="column is-one-quarter">
									<div class="field mb-4">
										<div class="control">
											<label class="label has-text-black">Father's Email</label>
											<input
												class="input has-background-white has-text-black"
												type="text"
												bind:value={student.fathersEmailAddress}
											/>
										</div>
									</div>
								</div>
								<div class="column auto">
									<div class="field mb-4">
										<div class="is-flex mt-6">
											<div class="control">
												<input
													class="checkbox"
													type="checkbox"
													bind:checked={student.isFatherDeceased}
												/>
											</div>
											<label class="label has-text-black ml-2">Deceased?</label>
										</div>
									</div>
								</div>

								<div class="column is-full has-background-light is-flex">
									<Icon className="mr-2 mt-1" icon={faInfoCircle}></Icon>
									<h2 class="title is-4 has-text-black">Guardian Information</h2>
								</div>
								<div class="column auto">
									<div class="field mb-4 mt-4">
										<div class="is-flex">
											<div class="control">
												<label class="checkbox has-text-black">
													<input
														type="checkbox"
														bind:checked={sameFather}
														on:change={() => fillGuardianWithFather(student)}
													/> Same as Father
												</label>
												<label class="checkbox has-text-black">
													<input
														type="checkbox"
														bind:checked={sameMother}
														on:change={() => fillGuardianWithMother(student)}
													/> Same as Mother
												</label>
											</div>
										</div>
									</div>
								</div>
								<!-- Row 7 -->
								<div class="column is-one-third">
									<div class="field mb-4">
										<div class="control">
											<label class="label has-text-black">Guardian's Name</label>
											<input
												class="input has-background-white has-text-black"
												type="text"
												bind:value={student.guardiansName}
											/>
										</div>
									</div>
								</div>
								<div class="column is-half">
									<div class="field mb-4">
										<div class="control">
											<label class="label has-text-black">Guardian's Address</label>
											<input
												class="input has-background-white has-text-black"
												type="text"
												bind:value={student.guardiansAddress}
											/>
										</div>
									</div>
								</div>
								<div class="column is-one-quarter">
									<div class="field mb-4">
										<div class="control">
											<label class="label has-text-black">Guardian's Contact</label>
											<input
												class="input has-background-white has-text-black"
												type="text"
												bind:value={student.guardiansContactNumber}
											/>
										</div>
									</div>
								</div>
								<div class="column is-one-quarter">
									<div class="field mb-4">
										<div class="control">
											<label class="label has-text-black">Guardian's Relationship</label>
											<input
												class="input has-background-white has-text-black"
												type="text"
												bind:value={student.guardianRelationship}
											/>
										</div>
									</div>
								</div>
								<div class="column is-half">
									<div class="field mb-4">
										<div class="control">
											<label class="label has-text-black">Guardian's Email</label>
											<input
												class="input has-background-white has-text-black"
												type="text"
												bind:value={student.guardiansEmailAddress}
											/>
										</div>
									</div>
								</div>
								<div class="column">
									<div class="field">
										<div class="buttons is-right">
											<button class="button" style="background: #08204F;color:white">Save</button>
										</div>
									</div>
								</div>
							</div>
						</fieldset>
					</form>
				</div>
			</div>
		</div>
	</section>
</div>
{#if errorMessage != '' || successMessage != ''}
	<Notification {errorMessage} {successMessage}></Notification>
{/if}

<style>
	.max-w-md {
		max-width: 28rem;
	}

	.max-w-lg {
		max-width: 32rem;
	}

	.max-w-xl {
		max-width: 36rem;
	}

	fieldset {
		border: 1px solid #dbdbdb;
		border-radius: 5px;
		padding: 10px;
		margin-bottom: 10px;
	}

	legend {
		font-size: 1.2rem;
		font-weight: bold;
		margin-bottom: 5px;
	}
</style>
