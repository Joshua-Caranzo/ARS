<script lang="ts">
    import { onMount, createEventDispatcher } from "svelte";
    import type { CallResultDto } from "../../../types/types";
    import type { EditUserDTO, SchoolDto, UserDTO, UserTypeDTO } from "../type";
    import Icon from "$lib/components/Icon.svelte";
    import { faArrowLeft } from "@fortawesome/free-solid-svg-icons";
    import { editUser, getSchool, getUserById, getUserType } from '../repo';
    import Notification from "$lib/components/Notification.svelte";
    import { goto } from "$app/navigation";

    const dispatch = createEventDispatcher();

    export let u: UserDTO;

    let errorMessage: string | undefined;
    let successMessage: string | undefined;

    let result: CallResultDto<UserDTO>;
    let user: EditUserDTO = {
        id: 0,
        userName: "",
        firstName: "",
        lastName: "",
        email: "",
        active: false,
        assignedSchoolId: 0,
        userTypeId: 0,
    };
    let userTypeListCallResult: CallResultDto<UserTypeDTO[]> = {
        message: "",
        data: [],
        data2: [],
        isSuccess: true,
        totalCount: null
    };
    let userTypes: UserTypeDTO[] = [];
    let schoolListCallResult: CallResultDto<SchoolDto[]> = {
        message: "",
        data: [],
        data2: [],
        isSuccess: true,
        totalCount: null
    };
    let schools: SchoolDto[] = [];
    let assignedId: number = 0;

    onMount(async () => {
        try {
            result = await getUserById(u.id);
            user = {
                ...result.data,
                id: u.id,
            };
            if (!result.isSuccess) {
                errorMessage = result.message;
            }
            userTypeListCallResult = await getUserType();
            userTypes = userTypeListCallResult.data;
            errorMessage = userTypeListCallResult?.message || '';
            schoolListCallResult = await getSchool();
            schools = schoolListCallResult.data;
            errorMessage = schoolListCallResult?.message || '';
            assignedId = user.assignedSchoolId;
        } catch (err: any) {
            errorMessage = err.message;
        }
    });

    async function handleSubmit(e: Event) {
        e.preventDefault();
        user = { ...user, id: u.id };
        user.assignedSchoolId = assignedId;
        try {
            const callResult: CallResultDto<object> = await editUser(user);

            if (callResult.isSuccess) {
                successMessage = callResult.message;
                setTimeout(() => {
                    handleClose();
                }, 2000);
            } else {
                errorMessage = callResult.message;
            }
        } catch (error: any) {
            errorMessage = error.message;
        }
    }

    function handleClose() {
        dispatch('close');
    }
</script>

<div class="container is-narrow">
    {#if result?.isSuccess}
        <div class="is-flex is-align-items-center mb-3">
            <button class="button is-link" on:click={handleClose}>
                <Icon icon={faArrowLeft}/>
            </button>
            <h1 class="subtitle ml-2 has-text-black">Edit User</h1>
        </div>

        <form class=" mx-auto" on:submit={handleSubmit}>
            <fieldset>
                <legend>Edit User</legend>
                <div class="columns">
                    <div class="column is-half">
                        <label class="label has-text-black">User Name</label>
                        <div class="control">
                            <input class="input has-background-white  has-text-black" type="text" bind:value={user.userName} required>
                        </div>
                    </div>
                    <div class="column is-half">
                        <label class="label has-text-black">Email</label>
                        <div class="control">
                            <input class="input has-background-white  has-text-black" type="email" bind:value={user.email} required>
                        </div>
                    </div>
                </div>
                <div class="columns">
                    <div class="column is-half">
                        <label class="label has-text-black">First Name</label>
                        <div class="control">
                            <input class="input has-background-white  has-text-black" type="text" bind:value={user.firstName} required>
                        </div>
                    </div>
                    <div class="column is-half">
                        <label class="label has-text-black">Last Name</label>
                        <div class="control">
                            <input class="input has-background-white  has-text-black" type="text" bind:value={user.lastName} required>
                        </div>
                    </div>
                </div>
                <div class="columns">
                    <div class="column is-half">
                        <label class="label has-text-black">User Type</label>
                        <select id="userTypeDropdown" class="input has-background-white has-text-black" bind:value={user.userTypeId}>
                            <option value={0}>-- SELECT --</option>
                            {#each userTypes as userType}
                                <option value={userType.id}>{userType.typeName}</option>
                            {/each}
                        </select>
                    </div>
                </div>
                <div class="block is-flex">
                    <div class="is-flex mr-4">
                        <label class="mr-2">Active</label>
                        <input type="checkbox" bind:checked={user.active}>
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
    .max-w-md {
        max-width: 28rem;
    }

    .max-w-lg {
        max-width: 32rem;
    }

    .max-w-xl {
        max-width: 36rem;
    }

    fieldset {
        border: 1px solid #dbdbdb;
        border-radius: 5px;
        padding: 10px;
        margin-bottom: 10px;
    }

    legend {
        font-size: 1.2rem;
        font-weight: bold;
        margin-bottom: 5px;
    }
</style>
