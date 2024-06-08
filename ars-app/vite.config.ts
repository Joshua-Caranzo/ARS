import { sveltekit } from '@sveltejs/kit/vite';
import { defineConfig } from 'vite';

export default defineConfig({
	plugins: [sveltekit()],
	server: {
		fs: {
			// Allow serving files from the specified directories
			allow: ['C:\\Users\\Joshua\\Documents\\ARS\\payroll\\payroll-app\\static\\images']
		  }
	  }
	
});
