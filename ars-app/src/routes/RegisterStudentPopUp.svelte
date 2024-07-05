<script lang="ts">
  import { createEventDispatcher, onMount } from "svelte";
  import type { CallResultDto } from "../types/types";
  import type { GradeLevel, SchoolDto, StrandDto, StudentFormData } from "./type";
  import { addStudent, getGradeLevels, getSchool, getStrands } from "./repo";
  import IconButton from '$lib/components/IconButton.svelte';
	import { faBackward, faInfoCircle } from "@fortawesome/free-solid-svg-icons";
	import Icon from "$lib/components/Icon.svelte";

  let student: StudentFormData = {
      id: 0,
      lastName: "",
      firstName: "",
      middleName: "",
      suffix:"",
      email: "",
      contactNumber: "",
      lrn: "",
      birthdate: new Date(),
      birthplace: "",
      civilStatus: "",
      religion: "",
      sex: "",
      mothersName: "",
      mothersAddress: "",
      mothersContactNumber: null, 
      mothersEmailAddress: null, 
      fathersName: "",
      fathersAddress: "",
      fathersContactNumber: null, 
      fathersEmailAddress: null, 
      guardiansName: "",
      guardiansAddress: "",
      guardiansContactNumber: null, 
      guardiansEmailAddress: null, 
      lastSchoolAttended: "",
      lastSchoolAttendedYear: "",
      gradeLevelId: 0,
      strandId: null,
      isMotherDeceased:false,
      isFatherDeceased:false,
      guardianRelationship:"",
      motherOccupation:null,
      fatherOccupation:null,
      studentAddress:""
  };

  let errorMessage: string | undefined;
  let successMessage: string | undefined;
  let schoolYearFrom: string;
  let schoolYearTo: string;
  let gradeLevelListCallResult: CallResultDto<GradeLevel[]> = {
      message: "",
      data: [],
      isSuccess: true,
      data2: [],
      totalCount: null
  };
  let gradeLevels: GradeLevel[] = [];
  let strandListCallResult: CallResultDto<StrandDto[]> = {
      message: "",
      data: [],
      isSuccess: true,
      data2: [],
      totalCount: null
  };
  let schools : SchoolDto[]= [];
  let strands: StrandDto[] = [];
  let sameMother: boolean = false;
  let sameFather: boolean = false;
  let selectSchool:boolean = true;
  let schoolId:number;
  let selectedSchool:string;
  let privacy:boolean = false;

  const dispatch = createEventDispatcher();

  const handleClose = () => {
    dispatch('close');
  };

  onMount(async () => {
      try {
          gradeLevelListCallResult = await getGradeLevels();
          gradeLevels = gradeLevelListCallResult.data;
          errorMessage = gradeLevelListCallResult?.message || '';
          strandListCallResult = await getStrands();
          strands = strandListCallResult.data;
          const schoolListCallResult = await getSchool();
          schools = schoolListCallResult.data;
          errorMessage = strandListCallResult?.message || '';
      } catch (err: any) {
          errorMessage = err.message;
      }
  });

  async function handleSubmit(e: Event) {
      e.preventDefault();
      student = { ...student };
      student.lastSchoolAttendedYear = `${schoolYearFrom}-${schoolYearTo}`;
      try {
      
              const callResult = await addStudent(student, schoolId, true);
              if (callResult.isSuccess) {
                  successMessage = callResult.message;
                  setTimeout(() => {
                      window.location.href = "/successfulregister";
                  }, 1000);
             
            }
            else {
                  errorMessage = callResult.message;
              }
      } catch (error: any) {
          errorMessage = error.message;
      }
  }

  function fillGuardianWithFather(s: StudentFormData) {
      if (sameFather == true) {
          student.guardiansName = s.fathersName ?? "";
          student.guardiansAddress = s.fathersAddress ?? "";
          student.guardiansContactNumber = s.fathersContactNumber ?? "";
          student.guardiansEmailAddress = s.fathersEmailAddress ?? "";
          student.guardianRelationship = "Father";
          sameMother=false;
      }
      else {
          student.guardiansName = "";
          student.guardiansAddress = "";
          student.guardiansContactNumber = "";
          student.guardiansEmailAddress = "";
          student.guardianRelationship= "";
      }
  }

  function fillGuardianWithMother(s: StudentFormData) {
      if (sameMother == true) {
          student.guardiansName = s.mothersName ?? "";
          student.guardiansAddress = s.mothersAddress ?? "";
          student.guardiansContactNumber = s.mothersContactNumber ?? "";
          student.guardianRelationship ="Mother";
          student.guardiansEmailAddress = s.mothersEmailAddress ?? "";
          sameFather = false;
      }
      else {
          student.guardiansName = "";
          student.guardiansAddress = "";
          student.guardiansContactNumber = "";
          student.guardiansEmailAddress = "";
          student.guardianRelationship = "";
      }
  }
 
  const handleSchoolChange = (event: Event) => {
    const selectElement = event.target as HTMLSelectElement;
    schoolId = parseInt(selectElement.value);
   
    const s = schools.find(sc => sc.id === schoolId) || null;
    selectedSchool = s?.schoolName ?? "";
    selectSchool = false;
};
 function OpenSchool()
 {
  selectSchool = true;
 }
</script>

<div class="modal is-active" on:close>
  <div class="modal-background"></div>
    <div class="modal-content modal-content-width">
      <div class="container">
          <div class="card" style="background-color: white;">
              <div class="card-content">
                <div class="is-flex is-justify-content-space-between">
                  <div>
                  <IconButton class="mr-2 button-blue" link small icon={faBackward} on:click={OpenSchool}></IconButton>
                  </div>
                  <p class="modal-card-title has-text-black mb-4 is-flex-grow-1 is-flex-shrink-1">{selectedSchool || ""}</p>
                  <button class="delete" aria-label="close" on:click={handleClose}></button>
              </div>
              
                  <form class="" on:submit={handleSubmit}>
                   
                      <fieldset>
                        <div class="columns mt-4">
                          <div class="column has-background-light is-flex">
                            <Icon className="mr-2 mt-1" icon={faInfoCircle}></Icon>
                              <h2 class="title is-4 has-text-black">Student Information</h2>
                          </div>
                      </div>
                          <div class="columns is-multiline">
                              <!-- Row 1 -->
                              <div class="column is-4">
                                <div class="field mb-4">
                                  <div class="control">
                                    <div class="is-flex"><label class="label has-text-black">Last Name</label>    <label class="label has-text-danger"> *</label></div>
                                    <input class="input has-background-white has-text-black" type="text" bind:value={student.lastName} required>
                                  </div>
                                </div>
                              </div>
                              <div class="column is-4">
                                <div class="field mb-4">
                                  <div class="control">
                                    <div class="is-flex"><label class="label has-text-black">First Name</label>    <label class="label has-text-danger"> *</label></div>
                                    <input class="input has-background-white has-text-black" type="text" bind:value={student.firstName} required>
                                  </div>
                                </div>
                              </div>
                              <div class="column is-3">
                                <div class="field mb-4">
                                  <div class="control">
                                    <label class="label has-text-black">Middle Name</label>
                                    <input class="input has-background-white has-text-black" type="text" bind:value={student.middleName}>
                                  </div>
                                </div>
                              </div>

                              <div class="column is-1">
                                <div class="field mb-4">
                                  <div class="control">
                                    <label class="label has-text-black">Suffix</label>
                                    <input class="input has-background-white has-text-black" type="text" bind:value={student.suffix}>
                                  </div>
                                </div>
                              </div>
                        
                              <!-- Row 2 -->
                              <div class="column is-3">
                                <div class="field mb-4">
                                  <div class="control">
                                    <div class="is-flex"><label class="label has-text-black">Email</label>    <label class="label has-text-danger"> *</label></div>
                                    <input class="input has-background-white has-text-black" type="email" bind:value={student.email} required>
                                  </div>
                                </div>
                              </div>
                              <div class="column is-3">
                                <div class="field mb-4">
                                  <div class="control">
                                    <div class="is-flex"><label class="label has-text-black">Contact Number</label>    <label class="label has-text-danger"> *</label></div>
                                    <input class="input has-background-white has-text-black" type="text" bind:value={student.contactNumber}>
                                  </div>
                                </div>
                              </div>
                              <div class="column is-3">
                                <div class="field ">
                                  <div class="control">
                                  
                                      <div class="is-flex"><label class="label has-text-black">Grade Level</label>    <label class="label has-text-danger"> *</label></div>
                                    <select class="input has-background-white has-text-black" bind:value={student.gradeLevelId}>
                                        <option value={0}>-- SELECT --</option>
                                        {#each gradeLevels as gradeLevel}
                                            <option value={gradeLevel.id}>{gradeLevel.level}</option>
                                        {/each}
                                    </select>
                                    </div>
                                    </div>
                                    </div>
                                    <div class="column is-3">
                                      <div class="field ">
                                        <div class="control">
                                        <label class="label has-text-black">Strand</label>
                                        <select class="input has-background-white has-text-black" disabled={student.gradeLevelId <= 13} bind:value={student.strandId}>
                                            <option value={null}>-- SELECT --</option>
                                            {#each strands as strand}
                                            <option value={strand.id}>   {strand.strandAbbrev ? strand.strandAbbrev : strand.strandName}</option>
                                            {/each}
                                        </select>
                                        </div>      
                                </div>
                              </div>
                               <!-- Row 3 -->
                               <div class="column is-3">
                                <div class="field mb-4">
                                  <div class="control">
                                    <div class="is-flex"><label class="label has-text-black">Student Address</label>    <label class="label has-text-danger"> *</label></div>
                                    <input class="input has-background-white has-text-black"  type="text" bind:value={student.studentAddress}>
                                  </div>
                                </div>
                              </div>

                               <div class="column is-3">
                                <div class="field mb-4">
                                  <div class="control">
                                    <label class="label has-text-black">LRN</label>
                                    <input class="input has-background-white has-text-black"  disabled={student.gradeLevelId <= 3} type="text" bind:value={student.lrn}>
                                  </div>
                                </div>
                              </div>
                               <div class="column is-2">
                                <div class="field mb-4">
                                  <div class="control">
                        
                                          <div class="is-flex"><label class="label has-text-black">Birthdate</label>    <label class="label has-text-danger"> *</label></div>
                                            <input class="input has-background-white has-text-black" type="date" bind:value={student.birthdate} required>
                                        </div>

                                        </div>
                                        </div>
                                        <div class="column is-2">
                                          <div class="field mb-4">
                                            <div class="control">
                                  
                                          <div class="is-flex"><label class="label has-text-black">Civil Status</label>    <label class="label has-text-danger"> *</label></div>
                                            <div class="select">
                                                <select class="has-background-white has-text-black" bind:value={student.civilStatus} required>
                                                    <option value="" disabled>Select Civil Status</option>
                                                    <option value="Single">Single</option>
                                                    <option value="Married">Married</option>
                                                </select>
                                            </div>
                                        </div>
                                    </div>  
                                  </div>    
                                  <div class="column is-2">
                      
                                    <div class="field mb-4">
                                    <div class="is-flex"><label class="label has-text-black">Sex</label>    <label class="label has-text-danger"> *</label></div>
                                      <div class="select">
                                          <select class="has-background-white has-text-black" bind:value={student.sex} required>
                                              <option value="" disabled>Select Sex</option>
                                              <option value="Male">Male</option>
                                              <option value="Female">Female</option>
                                          </select>
                                      </div>
                                  </div>
                                </div>   
                                  <div class="column is-3">
                                    <div class="field mb-4">
                                      <div class="control">
                                        <div class="is-flex"><label class="label has-text-black">Birth Place</label>    <label class="label has-text-danger"> *</label></div>
                                        <input class="input has-background-white has-text-black" type="text" bind:value={student.birthplace} required>
                                      </div>
                                    </div>
                                  </div>
                       
                              <!-- Row 4 -->
                              <div class="column is-3">
                      
                                <div class="field mb-4">
                                  <div class="control">
                                   <div class="is-flex"><label class="label has-text-black">Religion</label>    <label class="label has-text-danger"> *</label></div>
                                    <input class="input has-background-white has-text-black" type="text" bind:value={student.religion} required>
                                  </div>
                                </div>
                              </div>
                              <div class="column is-3">
                                <div class="field mb-4">
                                  <div class="control">
                                    <label class="label has-text-black">Last School Attended</label>
                                    <input class="input has-background-white has-text-black" type="text" bind:value={student.lastSchoolAttended} required>
                                  </div>
                                </div>
                              </div>
                              <div class="column is-one-quarter">
                                <div class="field mb-4">
                                    <div class="control">
                                        <label class="label has-text-black">Last School Year Attended</label>
                                        <div class="columns">
                                            <div class="column is-half">
                                                <input id="fromSchoolYear" class="input has-background-white has-text-black" type="number" min="2000" max="2100" placeholder="From (YYYY)" bind:value={schoolYearFrom} required>
                                            </div>
                                            <div class="column is-half">
                                                <input id="toSchoolYear" class="input has-background-white has-text-black" type="number" min="2010" max="2100" placeholder="To (YYYY)" bind:value={schoolYearTo} required>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            
                              <div class="column is-full has-background-light is-flex">
                                <Icon className="mr-2 mt-1" icon={faInfoCircle}></Icon>
                                  <h2 class="title is-4 has-text-black">Parents Information</h2>
                             </div>
                            <div class="column  is-half">
                              <div class="field mb-4">
                                <div class="control">
                                  <label class="label has-text-black">Mother's Name</label>
                                  <input class="input has-background-white has-text-black" type="text" bind:value={student.mothersName}>
                                </div>
                              </div>
                            </div>
                            <div class="column is-half">
                              <div class="field mb-4">
                                <div class="control">
                                  <label class="label has-text-black">Mother's Address</label>
                                  <input class="input has-background-white has-text-black" type="text" bind:value={student.mothersAddress}>
                                </div>
                              </div>
                            </div>
                            <div class="column  is-one-quarter">
                              <div class="field mb-4">
                                <div class="control">
                                  <label class="label has-text-black">Mother's Contact</label>
                                  <input class="input has-background-white has-text-black" type="text" bind:value={student.mothersContactNumber}>
                                </div>
                              </div>
                            </div>
                            <div class="column  is-one-quarter">
                              <div class="field mb-4">
                                <div class="control">
                                  <label class="label has-text-black">Mother's Occupation</label>
                                  <input class="input has-background-white has-text-black" type="text" bind:value={student.motherOccupation}>
                                </div>
                              </div>
                            </div>
                            <div class="column is-one-quarter">
                              <div class="field mb-4">
                                <div class="control">
                                  <label class="label has-text-black">Mother's Email</label>
                                  <input class="input has-background-white has-text-black" type="text" bind:value={student.mothersEmailAddress}>
                                </div>
                              </div>
                            </div>
                            <div class="column auto">
                            <div class="field mb-4 mt-4">
                              <div class="is-flex mt-6">
                                <div class="control">
                                  <input class="checkbox" type="checkbox" bind:checked={student.isMotherDeceased}>
                                </div>
                                <label class="label has-text-black ml-2"> Deceased?</label>
                              </div>
                            </div>
                            </div>
                            <div class="column  is-half">
                                <div class="field mb-4">
                                  <div class="control">
                                    <label class="label has-text-black">Father's Name</label>
                                    <input class="input has-background-white has-text-black" type="text" bind:value={student.fathersName}>
                                  </div>
                                </div>
                              </div>
                              <div class="column is-half">
                                <div class="field mb-4">
                                  <div class="control">
                                    <label class="label has-text-black">Father's Address</label>
                                    <input class="input has-background-white has-text-black" type="text" bind:value={student.fathersAddress}>
                                  </div>
                                </div>
                              </div>
                              <div class="column  is-one-quarter">
                                <div class="field mb-4">
                                  <div class="control">
                                    <label class="label has-text-black">Father's Contact</label>
                                    <input class="input has-background-white has-text-black" type="text" bind:value={student.fathersContactNumber}>
                                  </div>
                                </div>
                              </div>
                              <div class="column  is-one-quarter">
                                <div class="field mb-4">
                                  <div class="control">
                                    <label class="label has-text-black">Father's Occupation</label>
                                    <input class="input has-background-white has-text-black" type="text" bind:value={student.fatherOccupation}>
                                  </div>
                                </div>
                              </div>
                              <div class="column is-one-quarter">
                                <div class="field mb-4">
                                  <div class="control">
                                    <label class="label has-text-black">Father's Email</label>
                                    <input class="input has-background-white has-text-black" type="text" bind:value={student.fathersEmailAddress}>
                                  </div>
                                </div>
                              </div>
                              <div class="column auto">
                              <div class="field mb-4">
                                <div class="is-flex mt-6">
                                  <div class="control">
                                    <input class="checkbox" type="checkbox" bind:checked={student.isFatherDeceased}>
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
                                    <label class="checkbox has-background-white has-text-black">
                                      <input type="checkbox" bind:checked={sameFather} on:change={() => fillGuardianWithFather(student)}> Same as Father
                                    </label>
                                    <label class="checkbox has-background-white has-text-black">
                                      <input type="checkbox" bind:checked={sameMother} on:change={() => fillGuardianWithMother(student)}> Same as Mother
                                    </label>
                                </div>
                              </div>
                              </div>
                              </div>
                              <!-- Row 7 -->
                              <div class="column  is-one-third">
                                <div class="field mb-4">
                                  <div class="control">
                                    <div class="is-flex"><label class="label has-text-black">Guardian's Name</label>    <label class="label has-text-danger"> *</label></div>
                                    <input class="input has-background-white has-text-black" type="text" bind:value={student.guardiansName} required>
                                  </div>
                                </div>
                              </div>
                              <div class="column is-half">
                                <div class="field mb-4">
                                  <div class="control">
                                    <div class="is-flex"><label class="label has-text-black">Guardian's Address</label>    <label class="label has-text-danger"> *</label></div>
                                    <input class="input has-background-white has-text-black" type="text" bind:value={student.guardiansAddress} required>
                                  </div>
                                </div>
                              </div>
                              <div class="column  is-one-quarter">
                                <div class="field mb-4">
                                  <div class="control">
                                    <div class="is-flex"><label class="label has-text-black">Guardian's Contact</label>    <label class="label has-text-danger"> *</label></div>
                                    <input class="input has-background-white has-text-black" type="text" bind:value={student.guardiansContactNumber} required>
                                  </div>
                                </div>
                              </div>
                              <div class="column  is-one-quarter">
                                <div class="field mb-4">
                                  <div class="control">
                                    <div class="is-flex"><label class="label has-text-black">Guardian's Relationship</label>    <label class="label has-text-danger"> *</label></div>
                                    <input class="input has-background-white has-text-black" type="text" bind:value={student.guardianRelationship} required>
                                  </div>
                                </div>
                              </div>
                              <div class="column is-half">
                                <div class="field mb-4">
                                  <div class="control">
                                    <div class="is-flex"><label class="label has-text-black">Guardian's Email</label>    <label class="label has-text-danger"> *</label></div>
                                    <input class="input has-background-white has-text-black" type="text" bind:value={student.guardiansEmailAddress} required>
                                  </div>
                                </div>
                              </div>
                      </fieldset>
                      <div class="field mb-4 mt-4">
                        <div class="is-flex">
                          <div class="control">
                            <label class="checkbox has-background-light has-text-black p-4 ">
                              <input type="checkbox" class= "mr-2" bind:checked={privacy}>DATA PRIVACY SECTION - 
                              I understand that my personal data collected here maybe disclosed with {selectedSchool} or third parties for any legitimate purposes that it may serve. It's confidentiality shall be ensured and the use of which shall be governed by Republic Act 10173 otherwise known as "Data Privacy Act of 2012"

                          </label>
                          </div>
                          </div>
                          </div>
                      {#if errorMessage}
                          <div class="notification is-danger">
                              {errorMessage}
                          </div>
                      {/if}
                      {#if successMessage}
                          <div class="notification is-success">
                              {successMessage}
                          </div>
                      {/if}
                      <div class="buttons is-right">
                          <button type="button" class="button has-text-white button-red" on:click={handleClose}>Cancel</button>
                          <button type="submit" class="button button-blue has-text-white" disabled = {privacy == false}>Submit</button>
                      </div>
                  </form>
              </div>
          </div>
      </div>
  </div>
</div>
{#if selectSchool}
<div class="modal is-active">
  <div class="modal-background"></div>
    <div class="modal-content-margin-mobile">
      <div class="card" style="background-color: white;">
              <div class="card-content">
                <div class="is-flex justify-content-between">
                <p class="modal-card-title has-text-black mb-4">Select School</p>
                <button class="delete" aria-label="close" on:click={handleClose}></button>
                </div>
                <div class = "select">
                  <select class="input has-background-white has-text-black" on:change= {handleSchoolChange} bind:value={schoolId}>
                      <option disabled value={0}>-- SELECT --</option>
                      {#each schools as school}
                      <option value={school.id}>{school.schoolName}</option>
                      {/each}
                  </select>
                  </div>
                </div>
                </div>
      </div>
      </div>
   {/if} 
<style>

  .modal-background {
      background-color: #063F78;
      opacity:90%;
  }
  .card {
      
      background-color: white;

  }
  .modal-content-width {
    width: 80%; /* Set the width to 80% */
    max-width: 80%; /* Ensure the content does not exceed 80% width */
    margin: auto; /* Center the content horizontally */
  }

  input::placeholder {
  /* Apply your placeholder styles here */
  color: gray;
  font-style: italic;
}
.button-blue
{
  background-color: #063F78;
}

.button-red
{
  background-color: #b22222;;
}
</style>
