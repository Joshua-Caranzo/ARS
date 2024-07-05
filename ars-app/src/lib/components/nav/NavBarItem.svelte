<script lang="ts">
	import { page } from '$app/stores';
	import type { IconDefinition } from '@fortawesome/free-solid-svg-icons';
	import Icon from '../Icon.svelte';

	export let tag: keyof HTMLElementTagNameMap = 'a';
	export let href: string | undefined = undefined;
	export let name: string | undefined = undefined;
	export let icon: IconDefinition | undefined = undefined;
	export let hasDropdown: boolean = false;
	export let isHoverable: boolean = false;
	export let showDropdown: boolean = false;

	type Props = {
		name?: string | undefined;
		icon?: IconDefinition | undefined;
		hasDropdown?: boolean;
		isHoverable?: boolean;
		showDropdown?: boolean;
	};

	type $$Props = Props &
		(
			| { tag?: 'a'; href: string }
			| { tag: Exclude<keyof HTMLElementTagNameMap, 'a'> }
		);
</script>

<svelte:element
	this={tag}
	{href}
	class="navbar-item has-text-white"
	class:has-dropdown={hasDropdown}
	class:is-hoverable={isHoverable}
	class:show-dropdown={showDropdown}
	class:is-active={$page.url.pathname === href}
>
	<slot>
		{#if icon}
			<div style="position: relative;">
				<Icon {icon} />
				{#if name}
					<span>{name}</span>
				{/if}
			</div>
		{:else}
			{name}
		{/if}
	</slot>
</svelte:element>

<style lang="scss">
	.is-active {
		background-color: #0c2340 !important;
		color: white !important;
	}
	.navbar-item {
        transition: background-color 0.3s, color 0.3s;
		background-color: #063F78;
        /* Apply other common styles as needed */
    }

    .navbar-item:hover {
        background-color: #1a4167;
        color: #fff; 
    }
</style>