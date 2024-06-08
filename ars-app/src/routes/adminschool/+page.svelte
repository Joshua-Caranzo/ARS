    <script lang="ts">
        import Icon from "$lib/components/Icon.svelte";
        import Notification from "$lib/components/Notification.svelte";
        import { onMount } from "svelte";
        import type { CallResultDto } from "../../types/types";
        import type { School } from "./type";
        import { getSchoolById, editSchool} from "./repo";
        import { loggedInUser, shortcuts } from '$lib/store';
        import { faPenClip, faMapMarkerAlt, faUpload, faUser, faEnvelope, faClose } from "@fortawesome/free-solid-svg-icons";

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

        let editMode: boolean = false;
        let errorMessage: string | undefined;
        let successMessage: string | undefined;
        let result: CallResultDto<School>;
        let previewImage: string | null = null;
        let file: File | null = null;

        onMount(async () => {
            try {
                if ($loggedInUser) {
                    result = await getSchoolById(parseInt($loggedInUser?.uid));
                    school = result.data;
                    previewImage = school.imagePath ? `static/images/${school.imagePath}` : null;
                    errorMessage = result?.message || '';
                }
            } catch (err: any) {
                errorMessage = err.message;
            }
        });

        function toggleEditMode() {
            editMode = !editMode;
        }

        function saveChanges() {
            // Implement logic to save changes to the school details
            // This could involve making an API call to update the school information
        }

        function handleFileUpload(event: Event) {
        const input = event.target as HTMLInputElement;
        if (input.files && input.files.length > 0) {
            file = input.files[0]; // Update file variable
            previewImage = URL.createObjectURL(file);
        }
    }

    async function handleSubmit(e:Event){
        e.preventDefault();
        school = {...school};
        try {
             await editSchool(school, file);
        } catch (error:any) {
            errorMessage =  error.message;
        } 
    }
    
    </script>
   <form class=" mx-auto" on:submit={handleSubmit}>
    <fieldset>
    <section class="section" >
        <div class="container">
            <div class="card mx-auto " style="max-width: 600px; background-color: lightgray;">
                <div class="card-content">
          
                    <div class="profile-picture-container mb-4">
                        {#if previewImage}
                            <img src={previewImage} alt="School Picture" class="profile-picture">
                        {:else}
                            <Icon icon={faUser} className="icon is-large" />
                        {/if}
                       
                            <div class="file mt-3 is-link  ">
                                <label class="file-label">
                                    <input class="file-input" type="file" accept="image/*" on:change={handleFileUpload}>
                                    <span class="file-cta" style="background: #08204F;">
                                        <span class="file-icon">
                                            <Icon icon={faUpload} />
                                        </span>
                                        <span class="file-label">
                                            Choose a file…
                                        </span>
                                    </span>
                                </label>
                            </div>
                      
                    </div>
                    <div class="field mb-4">
             
                        <div class="control  has-text-centered">  
                                <label class="label is-title has-text-black is-size-3 ml-4" style="display: inline-block;">{school.schoolName}</label>
                        </div>
                    </div>
                    <div class="field mb-4">
                        <div class="control">
                          
                            <label class="label has-text-black">School Acronym</label>
                                <input class="input has-background-white has-text-black" type="text" bind:value={school.schoolAcronym} required>
                           
                        </div>
                    </div>
                    <div class="field mb-4">
                       
                        <div class="control">
                          
                             <label class="label has-text-black">School Address</label>
                                <input class="input has-background-white has-text-black" type="text" bind:value={school.schoolAddress} required>
                            
                        </div>
                    </div>
                    <div class="field mb-4">
                        
                        <div class="control">
                           
                            <label class="label has-text-black">School Email</label>
                                <input class="input has-background-white has-text-black" type="email" bind:value={school.schoolEmail} required>
                           
                        </div>
                    </div>
               
                        <div class="field">
                            <div class="control">
                                <button class="button" style="background: #08204F;">Save</button>
                            </div>
                        </div>
                  
                </div>
            </div>
        </div>
    </section>
    </fieldset>
    </form>
    {#if errorMessage || successMessage}
    <Notification errorMessage={errorMessage} successMessage={successMessage} />
    {/if}
    <style>
        .profile-picture-container {
            display: flex;
            align-items: center;
            justify-content: center;
            flex-direction: column;
        }

        .profile-picture {
            width: 150px;
            height: 150px;
            object-fit: cover;
            border-radius: 50%;
            margin-bottom: 1rem;
        }

        .icon.is-large {
            font-size: 4rem;
            color: #b5b5b5;
        }
    </style>
