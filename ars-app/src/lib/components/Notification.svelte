<!-- Notification.svelte -->
<script lang="ts">
    import { onMount, onDestroy } from "svelte";
    import { fade } from "svelte/transition";
  
    export let errorMessage: string = "";
    export let successMessage: string = "";
    let type: string = "is-danger";
  
    let notificationClass: string;
  
    $: if (errorMessage != "" || successMessage != "") {
      scheduleAutoHide();
    }
  
    onMount(() => {
        setNotificationClass();
        scheduleAutoHide();
    });
  
    onDestroy(() => {
      // Cleanup logic, if needed
    });
  
    function setNotificationClass() {
      notificationClass = `notification fixed ${type ? `is-${type}` : ""}`;
    }
  
    function handleButtonClick() {
      errorMessage = "";
      successMessage = "";
    }
  
    function scheduleAutoHide() {
      setTimeout(() => {
        errorMessage = "";
        successMessage = "";
      }, 5000); // Set the time in milliseconds (e.g., 3000 = 3 seconds)
    }
</script>
  
  {#if errorMessage != ""}
    <div transition:fade class="notification is-danger is-light">
      <button class="delete" on:click={handleButtonClick}></button>
      {errorMessage}
    </div>
  {/if}
  
  {#if successMessage != ""}
    <div transition:fade class="notification is-primary is-light">
      <button class="delete" on:click={handleButtonClick}></button>
      {successMessage}
    </div>
  {/if}
  
<style>
    .notification {
      position: absolute;
      top: 0px;
      right: 0px;
      z-index: 999999;
    }
</style>
  