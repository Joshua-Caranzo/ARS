<script lang="ts">
	import { page } from '$app/stores';

	export let dropdownActive = false;
	const toggleDropdown = () => (dropdownActive = !dropdownActive);
</script>

<a
	class="navbar-burger button-blue has-text-white"
	class:is-active={dropdownActive}
	on:click={toggleDropdown}
	href={$page.url.href}
>
	<span></span>
	<span></span>
	<span></span>
</a>