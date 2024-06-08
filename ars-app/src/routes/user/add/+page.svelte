
<script lang="ts">
	import Icon from "$lib/components/Icon.svelte";
	import { faArrowLeft } from "@fortawesome/free-solid-svg-icons";
	import type { CallResultDto } from "../../../types/types";
	import type { AddUserDTO, SchoolDto, UserTypeDTO } from "../type";
	import { addUser, getSchool, getUserType } from "../repo";
	import Notification from "$lib/components/Notification.svelte";
	import { onMount } from "svelte";

    let user: AddUserDTO = {
        userName: "",
        password: "",
        firstName: "",
        lastName: "",
        email: "",
        userTypeId:0,
        assignedSchoolId:0,
    };

    let confirmPassword = "";
    let isPasswordMatch = false;

    let errorMessage:string | undefined;
    let successMessage:string | undefined;
    let userTypeListCallResult:CallResultDto<UserTypeDTO[]> = {
        message:"",
        data: [],
        isSuccess:true,    
        data2:[],
        totalCount: null
    };
    let userTypes:UserTypeDTO[] = [];
    let schoolListCallResult:CallResultDto<SchoolDto[]> = {
        message:"",
        data: [],
        isSuccess:true,    
        data2:[],
        totalCount: null
    };
    let schools:SchoolDto[] = [];
       

        onMount(async () => {
        try {
            userTypeListCallResult = await getUserType();
            userTypes = userTypeListCallResult.data;
            errorMessage = userTypeListCallResult?.message || '';
            schoolListCallResult = await getSchool();
            schools = schoolListCallResult.data;
            errorMessage = schoolListCallResult?.message || '';
        } catch (err:any ) {
            errorMessage = err.message;
        }
    });

    $: {
        isPasswordMatch = confirmPassword === user.password

        if(confirmPassword === "" && user.password == "")
            isPasswordMatch = false;
    }

    async function handleSubmit(e:Event){
        e.preventDefault();
        console.log(user.assignedSchoolId)
        try {
            const callResult:CallResultDto<object> = await addUser(user);

            if(callResult.isSuccess){
                successMessage = callResult.message;
                setTimeout(() => {
                    window.location.href = "/user";
                }, 4000);
            }else{
                errorMessage = callResult.message;
            }
        } catch (error:any) {
            errorMessage = error.message;
        }
    }

</script>
<div class="is-flex is-align-items-center">
    <a class="button is-link" href="/user">
        <Icon icon={faArrowLeft}/>
    </a>      
    <h1 class="subtitle ml-2 has-text-black">Add User</h1>
</div>

<form class="mx-auto is-full" on:submit={handleSubmit}>
    <fieldset>
        <legend class="has-text-black">Add User</legend>

        <div class="columns">
            <div class="column is-half">
                <label class="label has-text-black">User Name</label>
                <div class="control">
                    <input class="input has-background-white has-text-black" type="text" bind:value={user.userName} required>
                </div>
            </div>
            <div class="column is-half">
                <label class="label has-text-black">Email</label>
                <div class="control">
                    <input class="input has-background-white has-text-black" type="email" bind:value={user.email} required>
                </div>
            </div>
        </div>
    
        <div class="columns">
            <div class="column is-half">
                <label class="label has-text-black">First Name</label>
                <div class="control">
                    <input class="input has-background-white has-text-black" type="text" bind:value={user.firstName} required>
                </div>
            </div>
            <div class="column is-half">
                <label class="label has-text-black">Last Name</label>
                <div class="control">
                    <input class="input has-background-white has-text-black" type="text" bind:value={user.lastName} required>
                </div>
            </div>
        </div>
    
        <div class="columns">
            <div class="column is-half">
                <label class="label has-text-black">Password</label>
                <div class="control">
                    <input class="input has-background-white has-text-black" type="password" bind:value={user.password} required>
                </div>
            </div>
            <div class="column is-half">
                <label class="label has-text-black">Confirm Password</label>
                <div class="control">
                    <input class="input has-background-white has-text-black" type="password" bind:value={confirmPassword} required>
                </div> 
            </div>
        </div>
        <div class="columns">
        <div class="column is-half">
            <label class="label has-text-black">User Type</label>
            <select class="input has-background-white has-text-black" bind:value={user.userTypeId}>
                <option value={0}>-- SELECT --</option>
                {#each userTypes as userType}
                    <option value={userType.id}>{userType.typeName}</option>
                {/each}
            </select>
        </div>
        <div class="column is-half">
            <label class="label has-text-black">Assign School</label>
            <select class="input has-background-white has-text-black" bind:value={user.assignedSchoolId}>
                <option value={0}>-- SELECT --</option>
                {#each schools as school}
                    <option value={school.id}>{school.schoolName}</option>
                {/each}
            </select>
        </div>
        </div>
        {#if (user.password === "")}
            <p class="is-size-8 has-text-danger mb-3">Password should not be empty</p>
        {:else if (!isPasswordMatch)}
            <p class="is-size-8 has-text-danger mb-3">Password should match</p>
        {/if}
        
        
        <div class="field">
            <div class="control">
            <button class="button is-link" disabled={!isPasswordMatch || user.assignedSchoolId === 0 || user.userTypeId === 0}>Add</button>
            </div>
        </div>
    </fieldset>
</form>

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