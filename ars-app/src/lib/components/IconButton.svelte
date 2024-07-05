<script lang="ts">
	import type { IconDefinition } from '@fortawesome/free-solid-svg-icons';
	import Icon from './Icon.svelte';
	export let text = '';
	export let icon: IconDefinition | undefined = undefined;
	let btnClass: string = '';
	export { btnClass as class };
	export let tag: string = 'button';
	export let title: string = '';
	export let success = false;
	export let danger = false;
	export let primary = false;
	export let link = false;
	export let small = false;
	export let tiny = false;
	export let info = false;
	export let warning = false;

	$: className =
		`button ` +
		btnClass +
		` ` +
		(success ? 'is-success' : '') +
		` ` +
		(danger ? 'is-danger' : '') +
		` ` +
		(primary ? 'is-primary' : '') +
		` ` +
		(link ? 'is-link' : '') +
		` ` +
		(small ? 'is-small' : '') +
		` ` +
		(tiny ? 'is-tiny' : '') +
		` ` +
		(info ? 'is-info' : '') +
		` ` +
		(warning ? 'is-warning' : '');
</script>

<svelte:element
	this={tag}
	class={className}
	on:click
	{title}
	role="button"
	tabindex="0"
	{...$$restProps}
>
	{#if icon}
		<Icon {icon} />
	{/if}
	{#if text || $$slots.default}
		<span>
			<slot>{text}</slot>
		</span>
	{/if}
</svelte:element>

<style>
	.is-tiny {
		font-size: 0.5rem;
	}
	.button-blue
	{
		background-color: #063F78;
	}
	.button-red
	{
		background-color: #b22222;
	}
</style>
