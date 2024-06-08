import { initAuth } from '$lib/auth/auth';
import type { LayoutLoad } from './$types';

export const load: LayoutLoad = async ({ url }) => {
	if (url.pathname === '/whoami') return;
	await initAuth();
};
