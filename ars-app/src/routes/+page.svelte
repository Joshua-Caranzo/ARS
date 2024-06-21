<script lang="ts">
	import { signIn } from '$lib/auth/oidcService';
	import IconButton from '$lib/components/IconButton.svelte';
	import { loggedInUser, shortcuts } from '$lib/store';
    import RegisterStudentPopUp from './RegisterStudentPopUp.svelte'
	import { faCashRegister, faRegistered, faSignIn, faUserPlus } from '@fortawesome/free-solid-svg-icons';


    let openRegister : boolean = false;

    function register()
    {
        openRegister = true;
    }
    function closeRegister()
    {
        openRegister = false;
    }

	function login() {
		signIn();
	}
    
</script>


<svelte:head>
	<title>ARS System</title>
</svelte:head>

{#if openRegister == true}
<RegisterStudentPopUp on:close={closeRegister}> </RegisterStudentPopUp>
{/if}

<section class="hero is-fullheight">
    <div class="hero-body">
        <div class="container has-text-centered">
            <!-- Logo and Welcome Message -->
            <figure class="image is-128x128 is-inline-block">
                <img src="/Carslogo.png" alt="Company Logo">
            </figure>
            <h1 class="title has-text-black">Welcome to ARS</h1>
            <h2 class="subtitle">Automated Record System</h2>

            <!-- Main Content -->
            <div class="content">
                {#if $loggedInUser}
                    <p>Welcome, {$loggedInUser.name}</p>
                {:else}
                    <!-- Call to Action Section -->
                    <div class="has-background-white">
                        <p>Please log in to continue.</p>
                        <div class="buttons is-centered">
                            <IconButton icon={faSignIn} class="button-blue has-background-white" on:click={login}>Login</IconButton>
                            <IconButton icon={faUserPlus} class="button-blue has-background-white" on:click={register}>Register</IconButton>
                        </div>
                    </div>
                {/if}
            </div>
        </div>
    </div>

    <!-- Footer -->
    <footer class="footer button-blue">
        <div class="content has-text-centered has-text-white">
            <p>
                <strong>Automated Record System</strong> by <a href="#">Congregation of the Augustinian Recollect Sisters - ICT team</a>. All rights reserved.
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

    .button-blue
	{
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
</style>