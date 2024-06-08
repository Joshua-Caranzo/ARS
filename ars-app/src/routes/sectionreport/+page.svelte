<script lang='ts'>
    import { onMount } from "svelte";
    import { loggedInUser } from '$lib/store';
    import { getSchoolById, getSchoolYearList, getCurrentSchoolTerm,  getTotalBySection, getTotalBySchool } from "./repo";
    import type { CallResultDto } from "../../types/types";
    import type { School, SchoolYear, GradeLevel, SectionDto, StudentTotal } from "./type";
    import { getGradeLevels } from "../repo";
    import { faChevronDown ,faPrint} from "@fortawesome/free-solid-svg-icons";
    import IconButton from "$lib/components/IconButton.svelte";

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

    let studentListCallResult: CallResultDto<SectionDto[]> = {
        message: "",
        data: [],
        isSuccess: true,    
        data2: [],
        totalCount: 0
    };

    let studentList: SectionDto[] = [];

    let totalListCallResult: CallResultDto<StudentTotal>;

    let total: StudentTotal;
    let currentIndex: number = 0;
    let maleCount: number = 0;
    let femaleCount: number = 0;


    onMount( () => {
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
                
                studentListCallResult = await getTotalBySection(parseInt($loggedInUser?.uid), sy);
                studentList = studentListCallResult.data;
                totalListCallResult = await getTotalBySchool(parseInt($loggedInUser?.uid), sy);
                total = totalListCallResult.data;
                console.log(total)
            }
        } catch (err: any) {
            errorMessage = err.message;
        }
    };

    const handleSchoolYearChange = async (event: Event) => {
        const selectElement = event.target as HTMLSelectElement;
        sy = parseInt(selectElement.value);
        currentIndex = syListCallResult.data.findIndex(item => item.id === sy);
        await refresh();
    };

    const getSchoolYearString = (schoolYear: SchoolYear) => {
        return `${schoolYear.fromSchoolTerm} - ${schoolYear.toSchoolTerm}`;
    };

    function afterPrintHandler() {
        reloadPage();
        const printSection = document.getElementById('print-section');
        if (printSection) {
            printSection.focus();
        }
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
    function reloadPage() {
        window.location.reload();
    }
</script>

<section class="section">
    <div class="container">
        <!-- School Information (Header) -->
        <div class="mb-5">
            <IconButton link icon={faPrint} on:click={printPage}>Print</IconButton>                
            <div id="print-section">
                    <div class="has-text-centered">
                        {#if previewImage}
                            <img src={previewImage} alt="School Picture" class="profile-picture" style="width: 120px; height: 100px;">
                        {/if}
                    </div>           
                    <div class="mb-4 has-text-centered">
                        <p class="is-size-4 has-text-black gothic-like-font">{school.schoolName}</p>
                        <p class="is-size-7 has-text-black">{school.schoolAddress}</p>
                        <p class="is-size-7 has-text-black">{school.schoolContactNum}</p>
                        <p class="is-size-7 has-text-black">{school.schoolEmail}</p>
                        <p class="is-size-5 has-text-black bold mt-6 gothic-like-font">BASIC EDUCATION DEPARTMENT</p>
                        <p class="is-size-6 has-text-black bold mt-6 gothic-like-font">Summary of Enrollment</p>
                        <div class="has-text-centered">
                            <div class="select-container">
                                <select class="select" on:change={handleSchoolYearChange}>
                                    {#each syListCallResult.data as schoolYear}
                                        <option value={schoolYear.id} selected={schoolYear.id === sy}>
                                            {getSchoolYearString(schoolYear)}
                                        </option>
                                    {/each}
                                </select>
                            </div>
                        </div>
                    </div>
        <!-- List of Students Section -->
                    <div class="mb-5">
                        {#if studentList}
                            <div style="overflow: auto;">
                                <table class="table is-fullwidth is-narrow is-bordered has-background-white my-custom-table">
                                    <thead>
                                        <tr>
                                            <th>Grade Level & Section</th>
                                            <th>Male</th>
                                            <th>Female</th>
                                            <th>Total</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {#each studentList as user}
                                    
                                            <tr>
                                                <td>{user.gradeLevel} - {user.sectionName}</td>
                                                <td>{user.totalMales}</td>
                                                <td>{user.totalFemales}</td>
                                                <td>{user.totalStudents}</td>
                                            </tr>
                                        {/each}     
                                    </tbody>
                                </table>
                            </div>
                        {/if}
                    </div>
                    {#if total}
                            <div class="mb-5">
                                <p class="is-size-6 has-text-black bold mt-6">Male: {total.totalMales}</p>
                                <p class="is-size-6 has-text-black bold mt-1">Female: {total.totalFemales}</p>
                                <p class="is-size-6 has-text-black bold mt-1">Total: {total.totalStudents}</p>
                                </div>
                    {/if}
            </div>
        </div>
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
        font-family: 'Franklin Gothic Medium', 'Arial', sans-serif;
    }
    .select-container::after {
        content: '';
        position: absolute;
        right: 0.5rem;
        pointer-events: none;
    }
    .icon {
        position: absolute;
        right: 0.5rem;
        pointer-events: none;
    }
    .my-custom-table {
        color: black;
    }
    .my-custom-table th,
    .my-custom-table td {
        color: black;
    }
</style>
