<script lang="ts">
	import { loggedInUser, shortcuts } from '$lib/store';
	import {
		faCog,
		faSignIn,
		faSignOut,
		faScrewdriverWrench,
		faUser,
		faUserTie,
		faFileInvoice,
		faRegistered,
		faCashRegister, faSchool, faGraduationCap, faSection,
		faUserCheck,
		faStickyNote,
		faAdd,
		faUserClock,
		faHouseUser,
		faHomeUser,
		faCodePullRequest,
		faUpLong,
	} from '@fortawesome/free-solid-svg-icons';
	import { createEventDispatcher, onMount } from 'svelte';
	import Icon from '../Icon.svelte';
	import NavBarItem from './NavBarItem.svelte';
	import NavBarBurger from './NavBarBurger.svelte';

	export let isLocalNetwork: boolean;
	export let brand: string;
	export let userTypeId: number;

	const dispatch = createEventDispatcher<{
		signIn: void;
		signOut: void;
	}>();

	function handleSignIn() {
		dispatch('signIn');
	}

	function handleSignOut() {
		dispatch('signOut');
	}

	let burgerActive = false;
	let searchFocused = false;
	let searchDropdownActive = false;

	$: searchFocused && (searchDropdownActive = true);
	$: !searchFocused && setTimeout(() => (searchDropdownActive = false), 100);
	let search = '';

	onMount(() => {
		const reloadFlag = localStorage.getItem('hasReloaded');
		if (!reloadFlag) {
			localStorage.setItem('hasReloaded', 'true');
			window.location.reload();
		}
	});
</script>
<svelte:head>
	<title>ARS System</title>
</svelte:head>
{#if $loggedInUser}
<nav class="navbar is-link is-fixed-top" aria-label="main navigation" style="margin-bottom: 15px; !important">
	<div class="navbar-brand">
		<NavBarItem href="/" name={brand} />
		<NavBarBurger bind:dropdownActive={burgerActive} />
	</div>
	<div id="navbar" class="navbar-menu" class:is-active={burgerActive} data-sveltekit-preload-data="tap">
		<div class="navbar-start">
			{#if userTypeId === 2}
			
			<NavBarItem href="/school" name="School" icon={faSchool} />
					<NavBarItem href="/user" name="User" icon={faUserTie} />	
					<NavBarItem href="/student" name="Student" icon={faGraduationCap} />
					<NavBarItem href="/schoolterm" name="School Term" icon={faUserClock} />
					
				
				<NavBarItem href="/reports" name="Reports" icon={faFileInvoice} />
			{/if}
			
			{#if userTypeId === 3}
			
			<NavBarItem href="/adminschool" name="School" icon={faSchool} />
					<NavBarItem href="/adminuser" name="User" icon={faUserTie} />
					<NavBarItem href="/adminsection" name="Section" icon={faSection} />
					<NavBarItem href="/student" name="Student" icon={faGraduationCap} />
					
					<NavBarItem hasDropdown isHoverable tag="div">
						<div class="navbar-link">
							<Icon icon={faFileInvoice} />
							<span>Reports</span>
						</div>
						<div class="navbar-dropdown">
							<NavBarItem href="/adminreport" name="Enrollment Reports"  />
							<NavBarItem href="/sectionreport" name="Section Reports"  />
							<NavBarItem href="/levelreport" name="Grade Level Reports"  />
							<NavBarItem href="/masterlist" name="Master Lists"  />
						</div>
					</NavBarItem>

				
			{/if}
			
			{#if userTypeId === 4}
			
			<NavBarItem href="/registrar" name="Registered" icon={faUserCheck} />
			<NavBarItem href="/registrartransaction" name="Transaction" icon={faCashRegister} />
			<NavBarItem href="/registrarenrolled" name="Enrolled" icon={faGraduationCap} />
			<NavBarItem hasDropdown isHoverable tag="div">
				<div class="navbar-link">
					<Icon icon={faFileInvoice} />
					<span>Reports</span>
				</div>
				<div class="navbar-dropdown">
					<NavBarItem href="/adminreport" name="Enrollment Reports"  />
					<NavBarItem href="/sectionreport" name="Section Reports"  />
					<NavBarItem href="/levelreport" name="Grade Level Reports"  />
					<NavBarItem href="/masterlist" name="Master Lists"  />
				</div>
			</NavBarItem>
			{/if}

			{#if userTypeId === 1}
			<NavBarItem href="/studentpage" name="Profile" icon={faHomeUser} />
			<NavBarItem href="/studentpage/request" name="Request To Move Up" icon={faUpLong} />
			{/if}
		</div>

		<div class="navbar-end">
			<NavBarItem hasDropdown isHoverable tag="div">
				<div class="navbar-link">
					<Icon icon={faUser} />
					<span>{$loggedInUser.name}</span>
				</div>
				<div class="navbar-dropdown">
					<NavBarItem href="/settings" name="User Settings" icon={faCog} />
				</div>
			</NavBarItem>
			<NavBarItem on:click={handleSignOut} href="/logout" name="Logout" icon={faSignOut} />
		</div>
	</div>
</nav>
{/if}
