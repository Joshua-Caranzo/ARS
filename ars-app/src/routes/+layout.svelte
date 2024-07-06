<script lang="ts">
	import 'bulma/css/bulma.min.css';
	import 'bulma-switch/dist/css/bulma-switch.min.css';
	import '../styles/site.scss';
	import Navbar from '$lib/components/nav/Navbar.svelte';

	import { signIn, signOut } from '$lib/auth/oidcService';
	import { loggedInUser, isLocal } from '$lib/store';

	import { page } from '$app/stores';
	import { onMount } from 'svelte';
	
	const unprotectedRoutes = ['/', '/signin-callback', '/successfulregister'];

	$: needsLogin = !unprotectedRoutes.includes($page.url.pathname);
	function login() {
		signIn();
	}

	onMount(()=>{
		if(needsLogin && !loggedInUser)
	{
		signIn();
	}
	});

async function waitForLogin(): Promise<void> {
    return new Promise<void>((resolve) => {
        const interval = setInterval(() => {
            if ($loggedInUser) {
                clearInterval(interval);
                resolve();
            }
        }, 100);
    });
}

</script>

{#if $loggedInUser}
<Navbar on:signIn={login} on:signOut={signOut} brand="ARS" userTypeId = {parseInt($loggedInUser.userTypeId)} isLocalNetwork={$isLocal} />
{/if}

<div class="main has-background-white">
	{#if $loggedInUser || !needsLogin}
		<slot />
	{:else}
		<div class="container mt-6">
			<p>(You must be logged in to continue)</p>
		</div>
	{/if}
</div>

<style>
	.main {
		padding: 1em; 
		min-height: 100vh; 
		box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); 
		border-radius: 8px;
	}
</style>