<script lang="ts">
	import { signIn } from '$lib/auth/oidcService';
	import IconButton from '$lib/components/IconButton.svelte';
	import { loggedInUser, shortcuts } from '$lib/store';
    import { faCashRegister, faRegistered, faSignIn, faUserPlus } from '@fortawesome/free-solid-svg-icons';


    let openRegister : boolean = false;

	function login() {
		signIn();
	}
    
</script>


<svelte:head>
	<title>ARS System</title>
</svelte:head>

<section class="hero is-fullheight">
    <div class="hero-body">
        <div class="container has-text-centered">
            <h1 class="title has-text-black">Your Registration is Successfull</h1>
            <div class="content">
               
                    <div class="dashboard">
                        <p>You may now log In to Continue.</p>
                        <IconButton icon={faSignIn} primary on:click={login}>Login</IconButton>   
                    </div>
               
            </div>
        </div>
    </div>
</section>