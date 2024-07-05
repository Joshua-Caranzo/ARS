<script lang='ts'>
    import { onMount } from "svelte";
    import { loggedInUser } from '$lib/store';
    import { getSchoolYearList, getCurrentSchoolTerm, getTotalBySection, getTotalBySchool, getLevelBySection, getAllSchools, GetTotalBySchoolSuperAdmin } from "./repo";
    import type { CallResultDto } from "../types/types";
    import type { School, SchoolYear, GradeLevel, SectionDto, StudentTotal, LevelDto } from "./type";

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

    let studentListCallResult: CallResultDto<LevelDto[]> = {
        message: "",
        data: [],
        isSuccess: true,    
        data2: [],
        totalCount: 0
    };

    let studentList: LevelDto[] = [];

    let totalListCallResult: CallResultDto<StudentTotal>;

    let total: StudentTotal;

    let schoolListResult: CallResultDto<School[]> = {
        message: "",
        data: [],
        isSuccess: true,    
        data2: [],
        totalCount: 0
    };
    let schools: School[] = [];

    onMount(async () => {
        initializeCurrentSchoolYear();
        refresh();
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
                schoolListResult = await getAllSchools();
                schools = schoolListResult.data;
                if (schools.length > 0 && school.id === 0) {
                    school = schools[0];
                }
                errorMessage = schoolListResult?.message || '';
                syListCallResult = await getSchoolYearList();
                studentListCallResult = await getLevelBySection(school.id, sy);
                studentList = studentListCallResult.data;
                totalListCallResult = await GetTotalBySchoolSuperAdmin(school.id, sy);
                total = totalListCallResult.data;
            }
        } catch (err: any) {
            errorMessage = err.message;
        }
    };

    const handleSchoolYearChange = async (event: Event) => {
        const selectElement = event.target as HTMLSelectElement;
        sy = parseInt(selectElement.value);
        await refresh();
    };

    const handleSchoolChange = async (event: Event) => {
        const selectElement = event.target as HTMLSelectElement;
        school = schools.find(s => s.id === parseInt(selectElement.value)) || school;
        await refresh();
    };

    const getSchoolYearString = (schoolYear: SchoolYear) => {
        return `${schoolYear.fromSchoolTerm} - ${schoolYear.toSchoolTerm}`;
    };


    function getRandomColor() {
        const colors = ['#3498db', '#f1c40f', '#e74c3c', '#2ecc71', '#9b59b6'];
        return colors[Math.floor(Math.random() * colors.length)];
    }
</script>

<div class="container">
    <div class="field is-horizontal" style="margin-bottom: 10px;">
            <label class="label has-text-black mr-2">Select School Year:</label>
 
                    <div class="select is-small">
                        <select class=" has-background-white has-text-black" on:change={handleSchoolYearChange}>
                            {#each syListCallResult.data as schoolYear}
                                <option value={schoolYear.id} selected={schoolYear.id === sy}>
                                    {getSchoolYearString(schoolYear)}
                                </option>
                            {/each}
                        </select>
                    </div>
    </div>

    <div class="field is-horizontal">
            <label class="label has-text-black mr-2">Select School:</label>
                    <div class="select is-small">
                        <select  class=" has-background-white has-text-black" on:change={handleSchoolChange}>
                            {#each schools as schoolOption}
                                <option value={schoolOption.id} selected={schoolOption.id === school.id}>
                                    {schoolOption.schoolName}
                                </option>
                            {/each}
                        </select>
                    </div>
    </div>

    {#if studentList}
    <div class="columns is-multiline is-mobile mt-6">
      {#each studentList as user}
      <div class="column auto" style="padding: 10px;"> 
            <div class="bar-container" style="margin-bottom: 10px;">
            <p class="value has-text-centered">{user.totalStudents}</p>
            <div class="bar" style={`height: ${user.totalStudents * 2}px; background-color: ${getRandomColor()};`}></div>
            <p class="label has-text-centered has-text-black">{user.gradeLevel}</p>
          </div>
        </div>
      {/each}
    </div>
    {/if}

</div>
    <!-- Total Students Section -->
    {#if total}
        <div class="mb-5 has-text-left">
            <p class="is-size-6 has-text-black bold mt-6">Male: {total.totalMales}</p>
            <p class="is-size-6 has-text-black bold mt-1">Female: {total.totalFemales}</p>
            <p class="is-size-6 has-text-black bold mt-1">Total: {total.totalStudents}</p>
        </div>
    {/if}
<style>
    .bar-container {
        display: flex;
        flex-direction: column;
        align-items: center;
        margin-bottom: 20px;
    }

    .bar {
        width: 40px; /* Adjust width as needed */
        transition: height 0.5s;
        border-radius: 4px;
        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
    }

    .label {
        margin-top: 5px;
        font-weight: bold;
        font-size: 14px;
    }

    .value {
        margin-top: 5px;
        font-size: 12px;
    }
</style>
