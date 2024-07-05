<script lang="ts">
    import Notification from "$lib/components/Notification.svelte";
    import { loggedInUser, shortcuts } from '$lib/store';
    import type { Student, EnrollmentHistory, SchoolYear } from "./type";
    import { getStudentList, getStudentById, getSchoolYearList, getCurrentSchoolTerm, getNotes } from "./repo";
    import type { CallResultDto } from "../../types/types";
    import { onMount } from "svelte";
    import Icon from "$lib/components/Icon.svelte";
    import { faArrowUp, faChevronLeft, faChevronRight, faEdit, faNoteSticky, faPlus, faPrint, faUser, faExchangeAlt, faExchange, faRecycle, faTransgender, faMoneyBillTransfer, faRefresh, faRedo } from "@fortawesome/free-solid-svg-icons";
    import IconButton from "$lib/components/IconButton.svelte";
    import EnrollPopUp from "./EnrollPopUp.svelte";
    import NotesPopUp from "./NotesPopUp.svelte";
    import { formatDate } from "date-fns";
	import Edit from "./edit/Edit.svelte";
	import html2canvas from "html2canvas";
	import jsPDF from "jspdf";
	import TransferStudent from "./TransferStudent.svelte";
	import TransferStrand from "./TransferStrand.svelte";

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
    let selectedRow: number | null = null;
    let openNotes:boolean =false;
    let studentNotesId:number = 0;
    let openTransferSection:boolean = false;

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
        sex: "",
        gradeLevelForSy:"",
        studentAddress:""
    };
    let id:number = 0;
    let sy: number | null = null;
    let gotoEdit:boolean = false;
    let notesCallResult:CallResultDto<string>;
    let notes:string|null = null;
    let openChangeStrand:boolean =false;

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

    function openNotesPopUp(studentId: number) {
        openNotes = !openNotes;
        studentNotesId = studentId
    }

    function closeNotesPopUp() {
        openNotes = false;            
    }

    function openSection(userId: number, gradeID: number, grade: string) {
        openTransferSection = !openTransferSection;
        studentId = userId;
        gradeId = gradeID;
        gradeLevel = grade;
    }

    function closeSection() {
        openTransferSection = false;            
    }

    function openStrand(userId: number) {
        openChangeStrand = !openChangeStrand;
        studentId = userId;
    }

    function closeStrand() {
        openChangeStrand = false;            
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
        selectedRow = student.id;
        studentinfo = student;
        studentEnrollmentListCallResult = await getStudentById(student.id);
        studentEnrollment = studentEnrollmentListCallResult.data;
        notesCallResult = await getNotes(student.id);
        notes = notesCallResult.data;
    }

    const printPage = () => {
    const element = document.getElementById("print-section");

    if (element) {
        // Configure html2canvas with scale factor (adjust scale as needed)
        const options = {
            scale: 2, // Apply a scale factor of 2x (adjust as needed)
        };

        html2canvas(element, options).then(canvas => {
            const imgData = canvas.toDataURL('image/png');
            const pdf = new jsPDF({
                unit: 'in', // Use inches for units
                format: [8.5, 13], // Long bond paper size: 8.5 x 13 inches
            });

            const imgProps = pdf.getImageProperties(imgData);
            const pdfWidth = pdf.internal.pageSize.getWidth() - 1; // Adjust margins as needed
            const pdfHeight = (imgProps.height * pdfWidth) / imgProps.width;

            // Add image to PDF with margins
            pdf.addImage(imgData, 'PNG', 0.5, 0.5, pdfWidth, pdfHeight); // Adjust margins as needed

            // Save the PDF file
            pdf.save(`Summary_of_Enrollment_${new Date().toISOString()}.pdf`);
        });
    }
};
    const handleSYChange = (event: Event) => {
        const selectElement = event.target as HTMLSelectElement;
        sy = parseInt(selectElement.value);
        fetchStudentList();
    };

    function goEdit(studentId: number) {
        id = studentId;
        gotoEdit = true;
    }

    function closeEdit() {
        gotoEdit = false;
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
    {#if isEnroll == true}
    <EnrollPopUp {studentId} gradeId={gradeId} gradeLevel = {gradeLevel} on:close = {closeEnroll}></EnrollPopUp>
    {/if}
    {#if openNotes == true}
    <NotesPopUp studentId = {studentNotesId} on:close = {closeNotesPopUp}></NotesPopUp>
    {/if}
    {#if openTransferSection == true}
    <TransferStudent {studentId}  gradeId={gradeId} gradeLevel = {gradeLevel} on:close = {closeSection}></TransferStudent>
    {/if}
    {#if openChangeStrand == true}
    <TransferStrand {studentId} on:close = {closeStrand}></TransferStrand>
    {/if}
    {#if gotoEdit == true}
    <Edit {id} on:close={closeEdit}></Edit>
    {:else}
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
        <th class="has-text-black">Action</th>
      </tr>
    </thead>
    <tbody>
        {#each students as user}
            <tr  class:selected={selectedRow === user.id}  on:click={() => getStudentInfo(user)}>
             
                <td>{user.firstName} {user.middleName || ''} {user.lastName}</td>
                
                <td>{user.gradeLevelForSy}</td>
                <td class="has-text-centered">
                    <IconButton class="button-blue has-text-white" icon={faEdit} on:click={() => goEdit(user.id)}>View/Edit</IconButton>
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

</div>
{#if studentinfo.id == 0}
<div class="column is-four-fifths">
    <section class="section">
        <div class="container">
            <div class="card mx-auto" style="background-color: lightgray;">
                <div class="card-content">
<p>No Student Information Found</p>
</div>
</div>
</div>
</section>
</div>
{:else}
<div class="column is-four-fifths">
<section class="section">
    <div class="container">
        <div class="card mx-auto" style="background-color: lightgray;">
            <div class="card-content">
                <div class="is-flex is-justify-content-space-between mb-6" id="printbutton">
                    <span></span>
                    <div>
                        <IconButton class="mr-2 button-blue has-text-white" icon={faNoteSticky} on:click={() => openNotesPopUp(studentinfo.id)}>Add Notes</IconButton>
                        <IconButton class="mr-2 button-blue has-text-white" icon={faExchangeAlt} on:click={() => openSection(studentinfo.id, studentinfo.gradeLevelId, studentinfo.gradeLevel)}>Transfer Section</IconButton>
                        {#if studentinfo.gradeLevelId > 13}
                        <IconButton class="mr-2 button-blue has-text-white" icon={faRedo}  on:click={() => openStrand(studentinfo.id)}>Change Strand</IconButton>
                        {/if}
                        <IconButton class="mr-2 button-blue has-text-white" icon={faArrowUp} on:click={() => openEnroll(studentinfo.id, studentinfo.gradeLevelId, studentinfo.gradeLevel)}>Move Up</IconButton>
                        <IconButton  class=" button-blue has-text-white" icon={faPrint} on:click={printPage}>Print</IconButton>
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
                
                <h2 class=" has-text-black mt-5 mb-2">Enrollment History:</h2>
                <table class="table is-bordered is-fullwidth has-background-white my-custom-table">
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
                            <td>{formatDate(user.dateEnrolled, 'MM/dd/yyyy')}</td>
                            <td>{user.level}</td>
                            <td>{user.sectionName}</td>
                        </tr>
                        {/each}
                    </tbody>
                </table>

                <h2 class="has-text-black mt-5 mb-2">Notes</h2>
                <div class="field"><textarea class="textarea has-background-white has-text-black" disabled value={notes}></textarea></div>
            </div>
        </div>
        
    </div>
    
</section>

</div>
{/if}
</div>
{/if}
    
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

    .selected {
        background-color: #f0f0f0; /* Or any color you want for the selected row */
    }
  </style>
  
