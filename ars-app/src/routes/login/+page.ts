import { browser } from '$app/environment';
import { signIn } from '$lib/auth/oidcService';
import type { PageLoad } from './$types';

export const load: PageLoad = async ({ url }) => {
	if (browser) {
		const redirectUrl = url.searchParams.get('redirectUrl') || undefined;
		window.sessionStorage.setItem(`redirectUrl`, redirectUrl || ``);
		await signIn();
	}
};
