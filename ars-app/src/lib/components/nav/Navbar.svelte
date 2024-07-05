
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
		faPersonArrowUpFromLine,
		faPerson,
		faUserEdit,
		faList,
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
	let itemActive = false;
	let itemName:string = "";
	$: searchFocused && (searchDropdownActive = true);
	$: !searchFocused && setTimeout(() => (searchDropdownActive = false), 100);

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
<nav class="navbar button-blue is-fixed-top" aria-label="main navigation" style="margin-bottom: 15px; !important">
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
				<NavBarItem href="/superadminreport" name="Reports" icon={faFileInvoice} />
				<NavBarItem href="/consolidated" name="Consolidated Report" icon={faList} />
			{/if}
			
			{#if userTypeId === 3}
				<NavBarItem href="/adminschool" name="School" icon={faSchool} />
				<NavBarItem href="/adminuser" name="User" icon={faUserTie} />
				<NavBarItem href="/adminsection" name="Section" icon={faSection} />
				<NavBarItem href="/adminstudent" name="Student" icon={faGraduationCap} />
				<NavBarItem hasDropdown isHoverable tag="div">
					<div class="navbar-link has-text-white">
						<span>Reports</span>
					</div>
					<div class="navbar-dropdown button-blue">
						<a class="navbar-item" href="/adminreport">
							<span>Enrollment Reports</span>
						</a>
						<a class="navbar-item" href="/sectionreport">
							<span>Section Reports</span>
						</a>
						<a class="navbar-item" href="/levelreport">
							<span>Grade Level Reports</span>
						</a>
						<a class="navbar-item" href="/masterlist">
							<span>Master Lists</span>
						</a>
					</div>
				</NavBarItem>
			{/if}
			
			{#if userTypeId === 4}
				<NavBarItem href="/registrar" name="Registered" icon={faUserCheck} />
				<NavBarItem href="/registrartransaction" name="Transaction" icon={faCashRegister} />
				<NavBarItem href="/registrarenrolled" name="Enrolled" icon={faGraduationCap} />
				<NavBarItem hasDropdown isHoverable tag="div">
					<div class="navbar-link has-text-white">
						<Icon icon={faFileInvoice} />
						<span>Reports</span>
					</div>
					<div class="navbar-dropdown button-blue">
						<a class="navbar-item" href="/adminreport">
							<span>Enrollment Reports</span>
						</a>
						<a class="navbar-item" href="/sectionreport">
							<span>Section Reports</span>
						</a>
						<a class="navbar-item" href="/levelreport">
							<span>Grade Level Reports</span>
						</a>
						<a class="navbar-item" href="/masterlist">
							<span>Master Lists</span>
						</a>
					</div>
				</NavBarItem>
			{/if}

			{#if userTypeId === 1}
				<NavBarItem href="/studentpage" name="Profile" icon={faHomeUser} />
				<NavBarItem href="/studentpage/request" name="Request To Move Up" icon={faUpLong} />
			{/if}
		</div>

		<div class="navbar-end ">
			<NavBarItem hasDropdown isHoverable tag="div">
				<div class="navbar-link has-text-white">
					<Icon icon={faUser} />
					<span>{$loggedInUser.name}</span>
				</div>
				<div class="navbar-dropdown button-blue">
					<a class="navbar-item" href="/editprofile">
						<Icon icon={faUserEdit} />
						<span>Edit Profile</span>
					</a>
				</div>
			</NavBarItem>
			<a class="navbar-item button-blue has-text-white" on:click={handleSignOut}>
				<Icon icon={faSignOut} />
				<span>Log Out</span>
			</a>
		</div>
	</div>
</nav>
{/if}

<style>
	.button-blue {
		background-color: #063F78;
	}

	.navbar-link:hover {
		background-color: #063F78;
	}

	.navbar-dropdown .navbar-item {
		background-color:  #063F78;
		color: white;
	}

	.navbar-dropdown .navbar-item:hover {
		background-color: #1a4167;
		color: white;
	}

	.navbar-item:hover {
		background-color: #1a4167;
		color: white;
	}
</style>