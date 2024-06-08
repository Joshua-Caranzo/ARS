<script lang="ts">
	import { onMount } from "svelte";
	import type { CallResultDto } from "../../types/types";
    import Icon from "$lib/components/Icon.svelte";
	import { faPlus } from "@fortawesome/free-solid-svg-icons";
	import ShiftListTable from "./ShiftListTable.svelte";
	import type { ShiftsDto } from "./type";
	import { getShiftList } from "./repo";

    let errorMessage: string | undefined;

    let shiftListCallResult:CallResultDto<ShiftsDto[]> = {
        message:"",
        data: [],
        isSuccess:true,    
        data2:[]
    };
    let shifts:ShiftsDto[] = [];

    onMount(async () => {
        try {
            shiftListCallResult= await getShiftList();
            shifts = shiftListCallResult.data;

            errorMessage = shiftListCallResult?.message || '';
        } catch (err:any ) {
            errorMessage = err.message;
        }
    });

    
</script>

<h1 class="subtitle">Shift List</h1>

<a class="button is-link mb-2" href="/shifts/add">
    <Icon icon={faPlus} className="mr-2"/>
    Add Shift
</a>
<ShiftListTable shifts={shiftListCallResult.data} message={errorMessage} isSuccess={shiftListCallResult.isSuccess}/>