<script lang="ts">
    import { signIn } from '$lib/auth/oidcService';
    import IconButton from '$lib/components/IconButton.svelte';
    import { loggedInUser, shortcuts } from '$lib/store';
    import RegisterStudentPopUp from './RegisterStudentPopUp.svelte';
    import { faSignIn, faUserPlus } from '@fortawesome/free-solid-svg-icons';
    import SuperAdmin from './SuperAdmin.svelte';
	import AdminDashboard from './AdminDashboard.svelte';
	import { onMount } from 'svelte';

    let openRegister: boolean = false;
    let loading:boolean = true;

    function register() {
        openRegister = true;
    }

    function closeRegister() {
        openRegister = false;
    }

    function login() {
        signIn();
    }

    onMount(() => {
        setTimeout(() => {
            loading = false;
        }, 1000);
    });
</script>

<svelte:head>
    <title>ARS System</title>
</svelte:head>

{#if openRegister}
    <RegisterStudentPopUp on:close={closeRegister}></RegisterStudentPopUp>
{/if}

<section class="hero is-fullheight">
    <div class="body mt-6">
        <div class="container has-text-centered">
            <div class="dashboard">
                {#if loading}
                <div class="loading">
                    <p>Loading...</p>
                    </div>
                    {:else}
                    {#if $loggedInUser}
                    {#if $loggedInUser.userTypeId == "1"}
                    <figure class="image is-128x128 is-inline-block mt-6">
                        <img src="/Carslogo.png" alt="Company Logo">
                    </figure>
                    <h1 class="title has-text-black">Welcome to ARS</h1>
                    <h3 class="subtitle has-text-black mt-2 mb-6">Automated Record System</h3>
                    {/if}
                        {#if $loggedInUser.userTypeId == "2"}
                        <p class="has-text-left has-text-black is-size-4 has-text-weight-bold mt-2 mb-6">Welcome {$loggedInUser.name}!</p>
                            <SuperAdmin />
                        {/if}
                        {#if $loggedInUser.userTypeId == "3" || $loggedInUser.userTypeId == "4"}
                        <p class="has-text-left has-text-black is-size-4 has-text-weight-bold mt-2 mb-6">Welcome {$loggedInUser.name}!</p>
                            <AdminDashboard />
                        {/if}
                {:else}
                    <figure class="image is-128x128 is-inline-block mt-6">
                        <img src="/Carslogo.png" alt="Company Logo">
                    </figure>
                    <h1 class="title has-text-black">Welcome to ARS</h1>
                    <h3 class="subtitle has-text-black mt-2 mb-6">Automated Record System</h3>
                    <!-- Call to Action Section -->
                    <div class="has-background-white mt-6 pt-6">
                        <p>Please log in to continue.</p>
                        <div class="buttons is-centered mt-4">
                            <IconButton icon={faSignIn} class="button-blue has-text-white" on:click={login}>Login</IconButton>
                            <IconButton icon={faUserPlus} class="button-blue has-text-white" on:click={register}>Register</IconButton>
                        </div>
                    </div>
                {/if}
                {/if}
              
            </div>
        </div>
    </div>

    <!-- Footer -->
    <footer class="footer button-blue">
        <div class="content has-text-centered has-text-white">
            <p>
                <strong class="has-text-white">Automated Record System</strong> by <a href="https://www.facebook.com/profile.php?id=100064420733729">Congregation of the Augustinian Recollect Sisters - ICT team</a>. All rights reserved.
            </p>
        </div>
    </footer>
</section>

<style global>
    .position-parent {
        position: relative;
    }

    .position-parent .position-left {
        position: absolute;
        right: 0;
        padding-right: inherit;
    }

    .position-parent .position-right {
        position: absolute;
        right: 0;
        padding-right: inherit;
    }

    @media only screen and (min-width: 1200px) {
        .position-parent .position-left-desktop {
            position: absolute;
            left: 0;
            padding-left: inherit;
        }

        .position-parent .position-right-desktop {
            position: absolute;
            right: 0;
            padding-right: inherit;
        }
    }

    .button-blue {
        background-color: #063F78;
    }

    .footer {
        padding: 10px 0; /* Adjust padding to make the footer smaller */
    }

    .footer .content {
        font-size: 12px; /* Adjust font size */
    }

    .footer p {
        margin: 0; /* Remove margin */
    }

    .loading {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
    font-size: 1.5rem;
    font-weight: bold;
}

</style>
