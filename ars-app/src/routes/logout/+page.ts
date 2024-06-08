import { browser } from '$app/environment';
import type { PageLoad } from './$types';
import { signOut } from '$lib/auth/oidcService';

export const load: PageLoad = async () => {
	if (browser) {
		await signOut();
	}
};
