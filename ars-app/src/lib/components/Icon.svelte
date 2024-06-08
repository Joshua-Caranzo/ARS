<script lang="ts">
	/* import Fa, { type FlipDir } from 'svelte-fa'; */
    import Fa from 'svelte-fa';
	import type { IconDefinition } from '@fortawesome/free-solid-svg-icons';
	import { createEventDispatcher } from 'svelte';

	export let icon: IconDefinition;
	export let className: string | undefined = undefined;
	/* export let flip: FlipDir | undefined = undefined; */
	export let flipVertical = false;
	export let rotate = '';
	export let small: boolean = false;
	export let left: boolean = false;
	export let right: boolean = false;
	export let clickable: boolean = false;

	const dispatch = createEventDispatcher<{
		click: void;
	}>();
</script>

<!-- svelte-ignore a11y-click-events-have-key-events -->
<!-- svelte-ignore a11y-no-static-element-interactions -->
<span
	class={'icon ' + className}
	class:flip-vertical={flipVertical}
	class:is-small={small}
	class:is-left={left}
	class:is-right={right}
	class:is-clickable={clickable}
	on:click={() => dispatch('click')}
>
	<Fa {icon}  {rotate} />
    <!-- No FlipDir found fro svelte-fa@3.0.4 -->
    <!-- {flip} -->
</span>

<style>
	.is-clickable {
		cursor: pointer;
	}
</style>