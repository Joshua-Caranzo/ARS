<script lang="ts">
    import { createEventDispatcher, onMount } from "svelte";
    import type { CallResultDto } from "../../types/types";
    import type { StudentFormData } from "./type";
    import { getStudentById } from "./repo";
    import { resolveRoute } from "$app/paths";

    export let studentId: number;
    let studentResult: CallResultDto<StudentFormData>;
    let student: StudentFormData;
    let errorMessage: string;

    const dispatch = createEventDispatcher();

    const handleClose = () => {
        dispatch('close');
    };

    function formatBirthDay(date: Date, formatType: string = 'Date'): string {
		const d = new Date(date);
		if (formatType === 'Date') {
			const year = d.getFullYear();
			const month = String(d.getMonth() + 1).padStart(2, '0');
			const day = String(d.getDate()).padStart(2, '0');
			return `${year}-${month}-${day}`;
		}
		return d.toISOString();
	}

    onMount(async () => {
        try {
            studentResult = await getStudentById(studentId, false);
            if (studentResult.isSuccess) {
                student = {
                    ...studentResult.data,
                    id: studentId
                };
            } else {
                errorMessage = studentResult.message;
            }
        } catch (err: any) {
            errorMessage = err.message;
        }
    });
</script>

<div class="modal is-active" on:close={handleClose}>
    <div class="modal-background"></div>
    <div class="modal-content modal-content-width">
        <div class="container">
            <div class="card" style="background-color: white;">
                <div class="card-content">
                    <button class="delete is-pulled-right" aria-label="close" on:click={handleClose}></button>
                    <div class="content">
                        <h2 class="title is-4 mb-4 has-text-black">Student Information</h2>

                        {#if studentResult != null}
                        <div class="columns">
                            <div class="column is-half">
                                <div class="field">
                                    <label class="label has-text-black">Last Name</label>
                                    <div class="control">
                                        <input class="input" type="text" value={student.lastName} readonly style="background: white; color: black;">
                                    </div>
                                </div>
                            </div>
                            <div class="column is-half">
                                <div class="field">
                                    <label class="label has-text-black">First Name</label>
                                    <div class="control">
                                        <input class="input" type="text" value={student.firstName} readonly style="background: white; color: black;">
                                    </div>
                                </div>
                            </div>
                        </div>
                        
                        <div class="columns">
                            <div class="column is-half">
                                <div class="field">
                                    <label class="label has-text-black">Middle Name</label>
                                    <div class="control">
                                        <input class="input" type="text" value={student.middleName || ''} readonly style="background: white; color: black;">
                                    </div>
                                </div>
                            </div>
                            <div class="column is-half">
                                <div class="field">
                                    <label class="label has-text-black">Suffix</label>
                                    <div class="control">
                                        <input class="input" type="text" value={student.suffix || ''} readonly style="background: white; color: black;">
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="columns">
                            <div class="column is-half">
                                <div class="field">
                                    <label class="label has-text-black">Email</label>
                                    <div class="control">
                                        <input class="input" type="email" value={student.email} readonly style="background: white; color: black;">
                                    </div>
                                </div>
                            </div>
                            <div class="column is-half">
                                <div class="field">
                                    <label class="label has-text-black">Contact Number</label>
                                    <div class="control">
                                        <input class="input" type="text" value={student.contactNumber || ''} readonly style="background: white; color: black;">
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="columns">
                            <div class="column is-half">
                                <div class="field">
                                    <label class="label has-text-black">Student Address</label>
                                    <div class="control">
                                        <input class="input" type="text" value={student.studentAddress} readonly style="background: white; color: black;">
                                    </div>
                                </div>
                            </div>
                            <div class="column is-half">
                                <div class="field">
                                    <label class="label has-text-black">LRN (Learner Reference Number)</label>
                                    <div class="control">
                                        <input class="input" type="text" value={student.lrn || ''} readonly style="background: white; color: black;">
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="columns">
                            <div class="column is-half">
                                <div class="field">
                                    <label class="label has-text-black">Birthdate</label>
                                    <div class="control">
                                        <input class="input" type="text" value={formatBirthDay(student.birthdate)} readonly style="background: white; color: black;">
                                    </div>
                                </div>
                            </div>
                            <div class="column is-half">
                                <div class="field">
                                    <label class="label has-text-black">Birthplace</label>
                                    <div class="control">
                                        <input class="input" type="text" value={student.birthplace} readonly style="background: white; color: black;">
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="columns">
                            <div class="column is-half">
                                <div class="field">
                                    <label class="label has-text-black">Civil Status</label>
                                    <div class="control">
                                        <input class="input" type="text" value={student.civilStatus} readonly style="background: white; color: black;">
                                    </div>
                                </div>
                            </div>
                            <div class="column is-half">
                                <div class="field">
                                    <label class="label has-text-black">Religion</label>
                                    <div class="control">
                                        <input class="input" type="text" value={student.religion} readonly style="background: white; color: black;">
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="columns">
                            <div class="column is-half">
                                <div class="field">
                                    <label class="label has-text-black">Sex</label>
                                    <div class="control">
                                        <input class="input" type="text" value={student.sex} readonly style="background: white; color: black;">
                                    </div>
                                </div>
                            </div>
                            <div class="column is-half">
                                <div class="field">
                                    <label class="label has-text-black">Guardian's Name</label>
                                    <div class="control">
                                        <input class="input" type="text" value={student.guardiansName} readonly style="background: white; color: black;">
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="columns">
                            <div class="column is-half">
                                <div class="field">
                                    <label class="label has-text-black">Guardian's Address</label>
                                    <div class="control">
                                        <input class="input" type="text" value={student.guardiansAddress} readonly style="background: white; color: black;">
                                    </div>
                                </div>
                            </div>
                            <div class="column is-half">
                                <div class="field">
                                    <label class="label has-text-black">Guardian's Contact Number</label>
                                    <div class="control">
                                        <input class="input" type="text" value={student.guardiansContactNumber || ''} readonly style="background: white; color: black;">
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="columns">
                            <div class="column is-half">
                                <div class="field">
                                    <label class="label has-text-black">Guardian's Email Address</label>
                                    <div class="control">
                                        <input class="input" type="text" value={student.guardiansEmailAddress || ''} readonly style="background: white; color: black;">
                                    </div>
                                </div>
                            </div>
                            <div class="column is-half">
                                <div class="field">
                                    <label class="label has-text-black">Guardian's Relationship</label>
                                    <div class="control">
                                        <input class="input" type="text" value={student.guardianRelationship} readonly style="background: white; color: black;">
                                    </div>
                                </div>
                            </div>
                        </div>

                        {:else}
                        <div class="notification is-danger">
                            {errorMessage}
                        </div>
                        {/if}
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<style>
    /* Additional custom styles can be added here */
</style>
