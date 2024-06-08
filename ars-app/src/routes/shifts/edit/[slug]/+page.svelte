<script lang="ts">
	import type { CallResultDto } from "../../../../types/types";
	import type { ShiftsDto, TimePolicyCode } from "../../type";
	import Icon from "$lib/components/Icon.svelte";
	import { faArrowLeft } from "@fortawesome/free-solid-svg-icons";
	import { onMount } from "svelte";
	import Notification from "$lib/components/Notification.svelte";
	import { editShift, getShiftListById, getTimePolicyCode } from "../../repo";

    export let data: {   
        id:number
    };

    let result: CallResultDto<ShiftsDto>;
    let shift: ShiftsDto = {
        id: 0,
        shftCode: '',
        shftDesc: null,
        shftTpolicyCode: '',
        shftType: null,
        shftMswipe: 0,
        shftRegHrs: 0,
        shftMandatorySwipe: 0,
        shftTmIn1: null,
        shftTmOut1: null,
        shftTmIn2: null,
        shftTmOut2: null,
        shftTmIn3: null,
        shftTmOut3: null,
        shftTmIn4: null,
        shftTmOut4: null,
        shftCreaBy: null,
        shftCreaDt: null,
        shftModiBy: null,
        shftModiDt: null
    };

    let timePolicyCodes: TimePolicyCode[] = [];

    let errorMessage:string | undefined;
    let successMessage:string | undefined;

    onMount(async () => {
        try {
            result = await getShiftListById(data.id);
            shift = {
                ...result.data,
                id: data.id,
            };

            var timePolicyCodesResponse = await getTimePolicyCode();

            if(timePolicyCodesResponse.isSuccess){
                timePolicyCodes = timePolicyCodesResponse.data
            }

            if(!result.isSuccess){
                errorMessage = result.message;
            }

        } catch (err:any ) {
            errorMessage = err.message;
        }
    });

    async function handleSubmit(e:Event){   
        try {
            const callResult:CallResultDto<object> = await editShift(shift);

            if(callResult.isSuccess){
                successMessage = callResult.message;
                setTimeout(() => {
                    window.location.href = "/shifts";
                }, 4000);
            }else{
                errorMessage = callResult.message;
            }
        } catch (error:any) {
            errorMessage =  error.message;
        }
    }

</script>

<div class="container is-narrow">
    {#if result?.isSuccess}
        <div class="is-flex is-align-items-center mb-3">
            <a class="button is-link" href="/shifts">
                <Icon icon={faArrowLeft}/>
            </a>
            <h1 class="subtitle ml-2">Edit Shift</h1>        
        </div>

        <form class=" mx-auto" on:submit|preventDefault={handleSubmit}>
            <fieldset>
                <legend>Edit Shift</legend>
                <div class="columns">
                    <div class="column is-half">
                        <label class="label">Shift Code</label>
                        <div class="control">
                            <input class="input" type="text" bind:value={shift.shftCode} required>
                        </div>
                    </div>
                    <div class="column is-half">
                        <label class="label">Shift Description</label>
                        <div class="control">
                            <input class="input" type="text" bind:value={shift.shftDesc}>
                        </div>
                    </div>
                </div>
            
                <div class="columns">
                    <div class="column is-half">
                        <label class="label">Shift Time Policy Code</label>
                        <select class="input" bind:value={shift.shftTpolicyCode} required>
                            <option value="">-- SELECT --</option>
                            {#each timePolicyCodes as tPolicyCode}
                                <option value={tPolicyCode.tpolCode}>{tPolicyCode.tpolDesc}</option>
                            {/each}
                        </select>
                    </div>
                    <div class="column is-half">
                        <label class="label">Shift Type</label>
                        <div class="control">
                            <input class="input" type="text" bind:value={shift.shftType} required>
                        </div>
                    </div>
                </div>
            
                <div class="columns">
                    <div class="column is-half">
                        <label class="label">Shift Mswipe</label>
                        <div class="control">
                            <input class="input" type="number" bind:value={shift.shftMswipe} required>
                        </div>
                    </div>
                    <div class="column is-half">
                        <label class="label">Shift RegHrs</label>
                        <div class="control">
                            <input class="input" type="number" bind:value={shift.shftRegHrs} required>
                        </div> 
                    </div>
                </div>
                
                <div class="columns">
                    <div class="column is-half">
                        <label class="label">Shift Mandator Swipe</label>
                        <div class="control">
                            <input class="input" type="number" bind:value={shift.shftMandatorySwipe}>
                        </div>
                    </div>
                </div>
                
                <div class="columns">
                    <div class="column is-half">
                        <label class="label">Shift Time In 1</label>
                        <div class="control">
                            <input class="input" type="text" bind:value={shift.shftTmIn1}>
                        </div>
                    </div>
                    <div class="column is-half">
                        <label class="label">Shift Out In 1</label>
                        <div class="control">
                            <input class="input" type="text" bind:value={shift.shftTmOut1}>
                        </div>
                    </div>
                </div>
            
                <div class="columns">
                    <div class="column is-half">
                        <label class="label">Shift Time In 2</label>
                        <div class="control">
                            <input class="input" type="text" bind:value={shift.shftTmIn2}>
                        </div>
                    </div>
                    <div class="column is-half">
                        <label class="label">Shift Out In 2</label>
                        <div class="control">
                            <input class="input" type="text" bind:value={shift.shftTmOut2}>
                        </div>
                    </div>
                </div>
        
                <div class="columns">
                    <div class="column is-half">
                        <label class="label">Shift Time In 3</label>
                        <div class="control">
                            <input class="input" type="text" bind:value={shift.shftTmIn3}>
                        </div>
                    </div>
                    <div class="column is-half">
                        <label class="label">Shift Out In 3</label>
                        <div class="control">
                            <input class="input" type="text" bind:value={shift.shftTmOut3}>
                        </div>
                    </div>
                </div>
        
                <div class="columns">
                    <div class="column is-half">
                        <label class="label">Shift Time In 4</label>
                        <div class="control">
                            <input class="input" type="text" bind:value={shift.shftTmIn4}>
                        </div>
                    </div>
                    <div class="column is-half">
                        <label class="label">Shift Out In 4</label>
                        <div class="control">
                            <input class="input" type="text" bind:value={shift.shftTmOut4}>
                        </div>
                    </div>
                </div>
            
                <div class="field">
                    <div class="control">
                        <button class="button is-link">Save</button>
                    </div>
                </div>
            </fieldset>
        </form>
    {/if}
</div>

{#if errorMessage != '' || successMessage != ''}
  <Notification {errorMessage} {successMessage}></Notification>
{/if}


<style>
    .max-w-md{
        max-width: 28rem;
    }	

    .max-w-lg{
        max-width: 32rem;
    }

    .max-w-xl{
        max-width: 36rem;
    }	

    fieldset 
    {
        border: 1px solid #dbdbdb;
        border-radius: 5px;
        padding: 10px;
        margin-bottom: 10px;
    }

    legend 
    {
        font-size: 1.2rem;
        font-weight: bold;
        margin-bottom: 5px;
    }
</style>

