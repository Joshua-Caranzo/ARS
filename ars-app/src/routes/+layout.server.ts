import type { LayoutServerLoad } from './$types';
import { initAuth } from '$lib/auth/auth';

export const prerender = true; 

export const load: LayoutServerLoad = async ({ url, cookies }) => {
	if (url.pathname === '/whoami') return;
	await initAuth(cookies);
};
