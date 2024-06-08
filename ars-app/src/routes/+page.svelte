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
            <h1 class="title has-text-black">Welcome to ARS</h1>
            <div class="content">
                {#if $loggedInUser}
                    <p>Welcome, {$loggedInUser.name}</p>
                {:else}
                    <div class="dashboard">
                        <p>Please login to continue.</p>
                        <IconButton icon={faSignIn} primary on:click={login}>Login</IconButton>   
                        <IconButton icon={faUserPlus} primary on:click={register}>Register as Student</IconButton>                
                      </div>
                {/if}
            </div>
        </div>
    </div>
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
</style>