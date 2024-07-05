<script lang='ts'>
    import { onMount } from "svelte";
    import { loggedInUser } from '$lib/store';
    import { getSchoolById, getSchoolYearList, getCurrentSchoolTerm, getSectionList, getConsolidatedReport } from "./repo";
    import type { CallResultDto } from "../../types/types";
    import type { School, SchoolYear, GradeLevel, StudentInfo, SchoolSectionDto, ConsolidatedReport } from "./type";
    import { getGradeLevels, getStrands } from "../repo";
    import { faChevronDown , faPrint } from "@fortawesome/free-solid-svg-icons";
    import IconButton from "$lib/components/IconButton.svelte";
    import type { StrandDto } from "../type";
	import html2canvas from "html2canvas";
	import jsPDF from "jspdf";

    let syListCallResult: CallResultDto<SchoolYear[]> = {
        message: "",
        data: [],
        isSuccess: true,    
        data2: [],
        totalCount: 0
    };
    let sy: number = 0;
	let errorMessage: string | undefined;
    let reportListCallResult: CallResultDto<ConsolidatedReport[]> = {
        message: "",
        data: [],
        isSuccess: true,    
        data2: [],
        totalCount: 0
    };

    let report:ConsolidatedReport[];
    let currentIndex:number;


    onMount(() => {
        initializeCurrentSchoolYear().then(() => {
            refresh();
        });
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
                syListCallResult = await getSchoolYearList();
                currentIndex = syListCallResult.data.findIndex(item => item.id === sy);
                reportListCallResult = await getConsolidatedReport(sy);
                report = reportListCallResult.data;
            }
        } catch (err: any) {
            errorMessage = err.message;
        }
    };

    const handleSchoolYearChange = async (event: Event) => {
        const selectElement = event.target as HTMLSelectElement;
        sy = parseInt(selectElement.value);
        currentIndex = syListCallResult.data.findIndex(item => item.id === sy);
     refresh();
    };

    const getSchoolYearString = (schoolYear: SchoolYear | undefined) => {
    return schoolYear ? `${schoolYear.fromSchoolTerm} - ${schoolYear.toSchoolTerm}` : "Unknown School Year";
    };
    function afterPrintHandler() {
        reloadPage();
        const printSection = document.getElementById('print-section');
        if (printSection) {
            printSection.focus();
        }
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
    function reloadPage() {
        window.location.reload();
    }
</script>

<section class="section">
    <div class="container">
        <div class="field is-flex is-justify-content-flex-end mt-2">
            <div class="control">
            <IconButton class="button-blue has-text-white" icon={faPrint} on:click={printPage}>Print</IconButton>
        </div>
        </div>
       {#if report}
        <div class="mb-5">
            <div id="print-section">
                <div style="overflow: auto;">
                    <table class="table is-fullwidth is-narrow is-bordered has-background-white my-custom-table">
                        <thead>
                            <tr>  
                                <th colspan="17">   
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

                                <th class="has-text-black is-size-7">School Name</th>
                                <th class="has-text-black is-size-7">Nursery</th>
                                <th class="has-text-black is-size-7">Kinder</th>
                                <th class="has-text-black is-size-7" >Kinder 2</th>
                                <th class="has-text-black is-size-7">Grade 1</th>
                                <th class="has-text-black is-size-7">Grade 2</th>
                                <th class="has-text-black is-size-7">Grade 3</th>
                                <th class="has-text-black is-size-7">Grade 4</th>
                                <th class="has-text-black is-size-7">Grade 5</th>
                                <th class="has-text-black is-size-7">Grade 6</th>
                                <th class="has-text-black is-size-7">Grade 7</th>
                                <th class="has-text-black is-size-7">Grade 8</th>
                                <th class="has-text-black is-size-7">Grade 9</th>
                                <th class="has-text-black is-size-7">Grade 10</th>
                                <th class="has-text-black is-size-7">Grade 11</th>
                                <th class="has-text-black is-size-7">Grade 12</th>
                            </tr>
                        </thead>
                        <tbody>
                            {#each report as r}
                                <tr>
                                    <td class="has-text-black is-size-7" style="white-space: nowrap;">{r.schoolName}</td>
                                    <td class="has-text-black is-size-7" style="white-space: nowrap;">{r.nursery}</td>
                                    <td class="has-text-black is-size-7" style="white-space: nowrap;">{r.kinder}</td>
                                    <td class="has-text-black is-size-7" style="white-space: nowrap;">{r.kinder2}</td>
                                    <td class="has-text-black is-size-7" style="white-space: nowrap;">{r.grade1}</td>
                                    <td class="has-text-black is-size-7" style="white-space: nowrap;">{r.grade2}</td>
                                    <td class="has-text-black is-size-7" style="white-space: nowrap;">{r.grade3}</td>
                                    <td class="has-text-black is-size-7" style="white-space: nowrap;">{r.grade4}</td>
                                    <td class="has-text-black is-size-7" style="white-space: nowrap;">{r.grade5}</td>
                                    <td class="has-text-black is-size-7" style="white-space: nowrap;">{r.grade6}</td>
                                    <td class="has-text-black is-size-7" style="white-space: nowrap;">{r.grade7}</td>
                                    <td class="has-text-black is-size-7" style="white-space: nowrap;">{r.grade8}</td>
                                    <td class="has-text-black is-size-7" style="white-space: nowrap;">{r.grade9}</td>
                                    <td class="has-text-black is-size-7" style="white-space: nowrap;">{r.grade10}</td>
                                    <td class="has-text-black is-size-7" style="white-space: nowrap;">{r.grade11}</td>
                                    <td class="has-text-black is-size-7" style="white-space: nowrap;">{r.grade12}</td>
                                </tr>
                            {/each}
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
        {/if}
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