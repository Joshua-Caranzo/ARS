<script lang="ts">
	import { initAuth } from '$lib/auth/auth';
	import { handleCallback } from '$lib/auth/oidcService';
	import { onMount } from 'svelte';

	let redir: string | null;

	onMount(async () => {
		redir = window.sessionStorage.getItem('redirectUrl');
		await handleCallback();
		await initAuth();
	});
</script>

<div>
	<p>Please wait...</p>
	{#if redir}
		<p>Redirecting to {redir}...</p>
	{/if}
</div>
