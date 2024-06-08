<script lang="ts">
    import Notification from "$lib/components/Notification.svelte";
    import { loggedInUser, shortcuts } from '$lib/store';
    import type { Student, EnrollmentHistory, SchoolYear } from "./type";
    import { getStudentList, getStudentById, getSchoolYearList, getCurrentSchoolTerm } from "./repo";
    import type { CallResultDto } from "../../types/types";
    import { onMount } from "svelte";
    import Icon from "$lib/components/Icon.svelte";
    import { faArrowUp, faChevronLeft, faChevronRight, faEdit, faPlus, faPrint, faUser } from "@fortawesome/free-solid-svg-icons";
    import IconButton from "$lib/components/IconButton.svelte";
    import EnrollPopUp from "./EnrollPopUp.svelte";
    import { formatDate } from "date-fns";

    let errorMessage: string | undefined;
    let searchQuery: string = "";
    let currentPage: number = 1;
    let rowsPerPage: number = 10;
    let totalCount: number | null = 0;
    let isEnroll: boolean = false;
    let studentId: number = 0;
    let gradeId: number = 0;
    let gradeLevel: string = "";
    let studentEnrollment: EnrollmentHistory[] = [];
    let schoolYears: SchoolYear[] = [];

    let studentEnrollmentListCallResult: CallResultDto<EnrollmentHistory[]> = {
        message: "",
        data: [],
        isSuccess: true,    
        data2: [],
        totalCount: 0
    };

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
    let studentinfo: Student = {
        id: 0,
        lastName: "",
        middleName: null,
        firstName: "",
        email: "",
        contactNumber: "",
        lrn: "",
        studentIdNumber: "",
        gradeLevel: "",
        strandName: "",
        gradeLevelId: 0,
        birthDate: new Date(),
        age: 0,
        birthPlace: "",
        civilStatus: "",
        religion: "",
        motherName: null,
        motherAddress: null,
        fatherName: null,
        fatherAddress: null,
        guardianName: "",
        guardianAddress: null,
        lastSchoolAttended: null,
        lastSchoolAttendedYear: null,
        motherContactNumber: null,
        fatherContactNumber: null,
        guardianContactNumber: null,
        motherEmailAddress: null,
        fatherEmailAddress: null,
        guardianEmailAddress: null,
        sex: ""
    };

    let sy: number | null = null;

    onMount(() => {
        // Initialize current school year asynchronously
        initializeCurrentSchoolYear();

        // Add event listener for afterprint event
        window.addEventListener('afterprint', afterPrintHandler);

        // Cleanup the event listener when component is destroyed
        return () => {
            window.removeEventListener('afterprint', afterPrintHandler);
        };
    });

    async function initializeCurrentSchoolYear() {
        const currentsyCallResult = await getCurrentSchoolTerm();
        if (currentsyCallResult.isSuccess) {
            sy = currentsyCallResult.data;
        } else {
            errorMessage = currentsyCallResult.message;
        }
        // Fetch initial data after setting the current school year
        fetchStudentList();
    }

    function afterPrintHandler() {
        reloadPage();
        const printSection = document.getElementById('print-section');
        if (printSection) {
            printSection.focus();
        }
    }

    function openEnroll(userId: number, gradeID: number, grade: string) {
        isEnroll = !isEnroll;
        studentId = userId;
        gradeId = gradeID + 1;
        gradeLevel = grade;
    }

    function closeEnroll() {
        isEnroll = false;            
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

    function reloadPage() {
        window.location.reload();
    }

    async function getStudentInfo(student: Student) {
        studentinfo = student;
        studentEnrollmentListCallResult = await getStudentById(student.id);
        studentEnrollment = studentEnrollmentListCallResult.data;
    }

    const printPage = () => {
        const printContent = document.getElementById("print-section");
        if (printContent) {
            const originalDocument = document.body.innerHTML;
            const content = printContent.innerHTML;
            document.body.innerHTML = content;
            window.print();
            document.body.innerHTML = originalDocument;
        }
    };

    const handleSYChange = (event: Event) => {
        const selectElement = event.target as HTMLSelectElement;
        sy = parseInt(selectElement.value);
        fetchStudentList();
    };
</script>
    {#if isEnroll == true}
    <EnrollPopUp {studentId} gradeId={gradeId} gradeLevel = {gradeLevel} on:close = {closeEnroll}></EnrollPopUp>
    {/if}
    <div>
        </div>
    <div class= "columns">
        <div class="column is-one-quarter">
             <h1 class="subtitle has-text-black">Registered Students</h1>
             <div class="field is-flex">
                <div class="control" style="flex: 1;">
                    <div class="select is-half">
                        <select class="input has-background-white has-text-black" on:change = {handleSYChange} bind:value={sy}>
                            <option value={0}>-- SELECT --</option>
                            {#each schoolYears as sys}
                            <option value={sys.id}>{sys.fromSchoolTerm} - {sys.toSchoolTerm}</option>
                            {/each}
                        </select>
                    </div>
                </div>
            </div>
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
          </tr>
        </thead>
        <tbody>
            {#each students as user}
                <tr on:click={() => getStudentInfo(user)}>
                 
                    <td>{user.firstName} {user.middleName || ''} {user.lastName}</td>
                    
                    <td>{user.gradeLevel}</td>
                    
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
  
</div>
<div class="column is-four-fifths">
    <section class="section">
        <div class="container">
            <div class="card mx-auto" style="background-color: lightgray;">
                <div class="card-content">
                    <div class="is-flex is-justify-content-space-between mb-6" id="printbutton">
                        <span></span>
                        <div>
                            <IconButton class="mr-2 is-link" icon={faArrowUp} on:click={() => openEnroll(studentinfo.id, studentinfo.gradeLevelId, studentinfo.gradeLevel)}>Move Up</IconButton>
                            <IconButton link icon={faPrint} on:click={printPage}>Print</IconButton>
                        </div>
                    </div>
                    
                    <div id="print-section">
                    <div class="columns is-centered"  >
                        <div class="column is-narrow has-text-centered">
                            <!-- User Icon -->
                            <Icon icon={faUser}></Icon>
                            <!-- Move Up Button -->
                            
                        </div>
                    </div>
                    <div class="columns is-multiline">
                        <div class="column is-one-third">
                            <label class="label has-text-black">Name: {studentinfo.firstName} {studentinfo.middleName || ''} {studentinfo.lastName}</label>
                        </div>
                        <div class="column is-one-third">
                            <label class="label has-text-black">Grade: {studentinfo.gradeLevel}</label>
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
                            <label class="label has-text-black">Strand Name: {studentinfo.strandName}</label>
                        </div>
                        <div class="column is-one-third">
                            <label class="label has-text-black">Birth Date: {new Date(studentinfo.birthDate).toLocaleDateString()}</label>
                        </div>
                        <div class="column is-one-third">
                            <label class="label has-text-black">Age: {studentinfo.age}</label>
                        </div>
                        <div class="column is-one-third">
                            <label class="label has-text-black">Birth Place: {studentinfo.birthPlace}</label>
                        </div>
                        <div class="column is-one-third">
                            <label class="label has-text-black">Civil Status: {studentinfo.civilStatus}</label>
                        </div>
                        <div class="column is-one-third">
                            <label class="label has-text-black">Religion: {studentinfo.religion}</label>
                        </div>
                        <div class="column is-one-third">
                            <label class="label has-text-black">Mother Name: {studentinfo.motherName || ''}</label>
                        </div>
                        <div class="column is-one-third">
                            <label class="label has-text-black">Mother Address: {studentinfo.motherAddress || ''}</label>
                        </div>
                        <div class="column is-one-third">
                            <label class="label has-text-black">Father Name: {studentinfo.fatherName || ''}</label>
                        </div>
                        <div class="column is-one-third">
                            <label class="label has-text-black">Father Address: {studentinfo.fatherAddress || ''}</label>
                        </div>
                        <div class="column is-one-third">
                            <label class="label has-text-black">Guardian Name: {studentinfo.guardianName}</label>
                        </div>
                        <div class="column is-one-third">
                            <label class="label has-text-black">Guardian Address: {studentinfo.guardianAddress || ''}</label>
                        </div>
                        <div class="column is-one-third">
                            <label class="label has-text-black">Mother Contact Number: {studentinfo.motherContactNumber || ''}</label>
                        </div>
                        <div class="column is-one-third">
                            <label class="label has-text-black">Father Contact Number: {studentinfo.fatherContactNumber || ''}</label>
                        </div>
                        <div class="column is-one-third">
                            <label class="label has-text-black">Guardian Contact Number: {studentinfo.guardianContactNumber || ''}</label>
                        </div>
                        <div class="column is-one-third">
                            <label class="label has-text-black">Mother Email Address: {studentinfo.motherEmailAddress || ''}</label>
                        </div>
                        <div class="column is-one-third">
                            <label class="label has-text-black">Father Email Address: {studentinfo.fatherEmailAddress || ''}</label>
                        </div>
                        <div class="column is-one-third">
                            <label class="label has-text-black">Guardian Email Address: {studentinfo.guardianEmailAddress || ''}</label>
                        </div>
                        <div class="column is-one-third">
                            <label class="label has-text-black">Sex: {studentinfo.sex}</label>
                        </div>
                    </div>
                    </div>
                    <h2 class="has-text-centered has-text-black mt-5">Enrollment History</h2>
                    <table class="table is-fullwidth is-bordered has-background-white my-custom-table">
                        <thead>
                            <tr>
                                <th class="has-text-black">Date Enrolled</th>
                                <th class="has-text-black">Grade Level</th>
                                <th class="has-text-black">Section</th>
                            </tr>
                        </thead>
                        <tbody>
                            {#each studentEnrollment as user}
                            <tr>
                                <td>{formatDate(user.dateEnrolled, 'MM:dd:yyyy')}</td>
                                <td>{user.level}</td>
                                <td>{user.sectionName}</td>
                            </tr>
                            {/each}
                        </tbody>
                    </table>
                </div>
            </div>
            
        </div>
    </section>
    </div>
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
    @media print {
        /* Hide the print button and table when printing */
        #print-section .is-pulled-right,
        #print-section table {
            display: none;
        }
    }
  </style>
  
