<script lang='ts'>
    import { onMount } from "svelte";
    import { loggedInUser } from '$lib/store';
    import { getSchoolById, getSchoolYearList, getCurrentSchoolTerm, getSectionList, getStudentMasterList } from "./repo";
    import type { CallResultDto } from "../../types/types";
    import type { School, SchoolYear, GradeLevel, StudentInfo, SchoolSectionDto } from "./type";
    import { getGradeLevels, getStrands } from "../repo";
    import { faChevronDown , faPrint } from "@fortawesome/free-solid-svg-icons";
    import IconButton from "$lib/components/IconButton.svelte";
    import type { StrandDto } from "../type";

    let previewImage: string | null = null;
    let school: School = {
        id: 0,
        schoolName: "",
        schoolAcronym: "",
        schoolAddress: "",
        schoolEmail: "",
        schoolContactNum: "",
        isActive: true,
        imagePath: null
    };
    let errorMessage: string | undefined;
    let syListCallResult: CallResultDto<SchoolYear[]> = {
        message: "",
        data: [],
        isSuccess: true,    
        data2: [],
        totalCount: 0
    };
    let sy: number = 0;

    let glListCallResult: CallResultDto<SchoolSectionDto[]> = {
        message: "",
        data: [],
        isSuccess: true,    
        data2: [],
        totalCount: 0
    };

    let studentListCallResult: CallResultDto<StudentInfo[]> = {
        message: "",
        data: [],
        isSuccess: true,    
        data2: [],
        totalCount: 0
    };
    
    let strandListCallResult: CallResultDto<StrandDto[]> = {
        message: "",
        data: [],
        isSuccess: true,    
        data2: [],
        totalCount: 0
    };

    let studentList: StudentInfo[] = [];
    let gl: number = 1;
    let str: number | null = null;
    let currentIndex: number = 0;
    let maleCount: number = 0;
    let femaleCount: number = 0;
    let searchQuery: string = "";
    onMount(() => {
        initializeCurrentSchoolYear();
        refresh();
        window.addEventListener('afterprint', afterPrintHandler);

        // Cleanup the event listener when component is destroyed
        return () => {
            window.removeEventListener('afterprint', afterPrintHandler);
        };
    });

    const initializeCurrentSchoolYear = async () => {
        const currentsyCallResult = await getCurrentSchoolTerm();
        if (currentsyCallResult.isSuccess) {
            sy = currentsyCallResult.data;
        } else {
            errorMessage = currentsyCallResult.message;
        }
    }

    const refresh = async () => {
        try {
            if ($loggedInUser) {
                const result = await getSchoolById(parseInt($loggedInUser?.uid));
                school = result.data;
                previewImage = school.imagePath ? `static/images/${school.imagePath}` : null;
                errorMessage = result?.message || '';
                syListCallResult = await getSchoolYearList();
                currentIndex = syListCallResult.data.findIndex(item => item.id === sy);
                glListCallResult = await getSectionList(parseInt($loggedInUser?.uid));
                studentListCallResult = await getStudentMasterList(parseInt($loggedInUser?.uid), sy, gl, str,searchQuery);
                studentList = studentListCallResult.data;
                strandListCallResult = await getStrands();

                maleCount = studentList.filter(student => student.sex === 'Male').length;
                femaleCount = studentList.filter(student => student.sex === 'Female').length;
            }
        } catch (err: any) {
            errorMessage = err.message;
        }
    };

    const handleSchoolYearChange = async (event: Event) => {
        const selectElement = event.target as HTMLSelectElement;
        sy = parseInt(selectElement.value);
        currentIndex = syListCallResult.data.findIndex(item => item.id === sy);
        str = null;
     refresh();
    };

    const handleGradeLevelChange = async (event: Event) => {
        const selectElement = event.target as HTMLSelectElement;
        gl = parseInt(selectElement.value);
        str = null;
        await refresh();
    };

    const handleStrandChange = async (event: Event) => {
        const selectElement = event.target as HTMLSelectElement;
        str = selectElement.value === '' ? null : parseInt(selectElement.value);
        await refresh();
    };

    const getSchoolYearString = (schoolYear: SchoolYear | undefined) => {
    return schoolYear ? `${schoolYear.fromSchoolTerm} - ${schoolYear.toSchoolTerm}` : "Unknown School Year";
}   ;
const getGradeLevelString = (gradeLevel: SchoolSectionDto | undefined) => {
    return gradeLevel ? gradeLevel.level : "Unknown Grade Level";
};


const getStrandsString = (strand: StrandDto | undefined) => {
    return strand ? strand.strandAbbrev || strand.strandName : "Unknown Strand";
};


    function afterPrintHandler() {
        reloadPage();
        const printSection = document.getElementById('print-section');
        if (printSection) {
            printSection.focus();
        }
    }

    function exportToCsv(
    data: StudentInfo[],
    schoolYear: string,
    gradeLevel: string,
    strand: string | null
        ) {
            const csvContent = "data:text/csv;charset=utf-8," 
                + `School Year,${schoolYear}\n`
                + `Grade Level,${gradeLevel}\n`
                + "Strand," + (strand || "") + "\n" // If strand is null, use an empty string
                + "Student ID,Last Name,First Name,Middle Name,Sex,Birth Date,LRN,Contact Number,Religion,Father's Name,Father's Occupation,Mother's Name,Mother's Occupation\n" 
                + data.map(student => [
                    student.studentIdNumber,
                    student.lastName,
                    student.firstName,
                    student.middleName || "",
                    student.sex,
                    formatDate(student.birthDate),
                    student.lrn || "",
                    student.contactNumber,
                    student.religion,
                    student.fatherName || "",
                    student.fatherOccupation || "",
                    student.motherName || "",
                    student.motherOccupation || ""
                ].join(",")).join("\n");

            const encodedUri = encodeURI(csvContent);
            const link = document.createElement("a");
            link.setAttribute("href", encodedUri);
            link.setAttribute("download", "student_list.csv");
            document.body.appendChild(link); // Required for Firefox
            link.click();
        }


    function reloadPage() {
        window.location.reload();
    }

    async function handleSearch() {
        await refresh();
    }

    function formatDate(dateString: Date) {
    const date = new Date(dateString);
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    const year = String(date.getFullYear());
    return `${month}-${day}-${year}`;
}

</script>

<section class="section">
    <div class="container">
        <div class="field is-flex">
            <div class="control mr-4" style="flex: 1;">
                <input class="input has-background-white has-text-black" type="text" placeholder="Search..." bind:value={searchQuery} on:input={handleSearch} />
            </div>
        
            <IconButton class="button-blue has-text-white" icon={faPrint} on:click={() => exportToCsv(studentList, getSchoolYearString(syListCallResult.data[currentIndex]), getGradeLevelString(glListCallResult.data.find(gls => gls.gradeLevelId === gl)), str ? getStrandsString(strandListCallResult.data.find(strand => strand.id === str)) : null)}>Print</IconButton>

        </div>
        
        <!-- List of Students Section -->
        <div class="mb-5">
            
                <div style="overflow: auto;">
                    <table class="table is-fullwidth is-narrow is-bordered has-background-white my-custom-table">
                        <thead>
                            <tr>  
                                <th colspan="14">   
                                <div class="select-container">
                                    <label for="schoolYearSelect" class="select-label has-text-black">School Year:</label>
                                    <select class="select" on:change={handleSchoolYearChange}>
                                        {#each syListCallResult.data as schoolYear}
                                            <option value={schoolYear.id} selected={schoolYear.id === sy}>
                                                {getSchoolYearString(schoolYear)}
                                            </option>
                                        {/each}
                                    </select>
                                </div>
                                </th>
                                </tr>
                                <tr>
                                    <th colspan="14">
                                     
                                        <div class="select-container">
                                            <label for="gradeLevelSelect" class="select-label has-text-black">Grade Level:</label>
                                    <select class="select" on:change={handleGradeLevelChange}>
                                        {#each glListCallResult.data as gradeLevel}
                                            <option value={gradeLevel.gradeLevelId} selected={gradeLevel.gradeLevelId === gl}>
                                                {getGradeLevelString(gradeLevel)}
                                            </option>
                                        {/each}
                                    </select>
                                </div>
                               </th>
                            </tr>
                            {#if gl >= 14}
                            <tr>
                                <th colspan="14">
                              
                                <div class="select-container">
                                    <label for="strandSelect" class="select-label has-text-black">Strand:</label>
                                    <select class="select " on:change={handleStrandChange}>
                                        <option value="">Select Strand</option>
                                        {#each strandListCallResult.data as strand}
                                            <option value={strand.id} selected={strand.id === str}>
                                                {getStrandsString(strand)}
                                            </option>
                                        {/each}
                                    </select>
                                </div>
                           
                            </th>
                            </tr>
                            {/if}
                            <tr >
                                <th class="has-text-black is-size-7">Student ID</th>
                                <th class="has-text-black is-size-7">Last Name</th>
                                <th class="has-text-black is-size-7">First Name</th>
                                <th class="has-text-black is-size-7" >Middle Name</th>
                                <th class="has-text-black is-size-7">Sex</th>
                                <th class="has-text-black is-size-7">Birth Date</th>
                                <th class="has-text-black is-size-7">LRN</th>
                                <th class="has-text-black is-size-7">Contact Number</th>
                                <th class="has-text-black is-size-7">Religion</th>
                                <th class="has-text-black is-size-7">Father's Name</th>
                                <th class="has-text-black is-size-7">Father's Occupation</th>
                                <th class="has-text-black is-size-7">Mother's Name</th>
                                <th class="has-text-black is-size-7">Mother's Occupation</th>
                            </tr>
                        </thead>
                        <tbody>
                            {#each studentList as user}
                                <tr>
                                    <td class="has-text-black is-size-7" style="white-space: nowrap;">{user.studentIdNumber}</td>
                                    <td class="has-text-black is-size-7" style="white-space: nowrap;">{user.lastName}</td>
                                    <td class="has-text-black is-size-7" style="white-space: nowrap;">{user.firstName}</td>
                                    <td class="has-text-black is-size-7" style="white-space: nowrap;">{user.middleName || ""}</td>
                                    <td class="has-text-black is-size-7" style="white-space: nowrap;">{user.sex}</td>
                                    <td class="has-text-black is-size-7" style="white-space: nowrap;">{formatDate(user.birthDate)}</td>
                                    <td class="has-text-black is-size-7" style="white-space: nowrap;">{user.lrn || ""}</td>
                                    <td class="has-text-black is-size-7" style="white-space: nowrap;">{user.contactNumber}</td>
                                    <td class="has-text-black is-size-7" style="white-space: nowrap;">{user.religion}</td>
                                    <td class="has-text-black is-size-7" style="white-space: nowrap;">{user.fatherName || ""}</td>
                                    <td class="has-text-black is-size-7" style="white-space: nowrap;">{user.fatherOccupation || ""}</td>
                                    <td class="has-text-black is-size-7" style="white-space: nowrap;">{user.motherName || ""}</td>
                                    <td class="has-text-black is-size-7" style="white-space: nowrap;">{user.motherOccupation || ""}</td>
                                </tr>
                            {/each}
                        </tbody>
                    </table>
                </div>
            
        </div>
        <!-- Additional Sections (if any) -->
        <!-- For example, you can add sections for academic performance, attendance, etc. -->
    </div>
</section>

<style>
    .gothic-like-font {
        font-family: 'Franklin Gothic Medium', 'Arial', sans-serif;
    }
   
    .select {
        font-size: 1rem;
        padding: 0.5rem;
        border: none;
        border-radius: 4px;
        outline: none;
        appearance: none;
        background: transparent; 
    }
    .select-label {
    display: inline-block;
    margin-top: 8px; /* Adjust as necessary */
}
.button-blue
	{
		background-color: #063F78;
        color:white;
	}
</style>