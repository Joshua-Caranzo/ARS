import adapterStatic from '@sveltejs/adapter-static';
import { vitePreprocess } from '@sveltejs/vite-plugin-svelte';

const dynamicRoutesToExclude = [
  '/adminsection/edit/[slug]',
  '/adminuser/edit/[slug]',
  '/registrar/edit/[slug]',
  '/schoolterm/edit/[slug]',
  '/school/edit/[slug]',
  '/shifts/edit/[slug]',
  '/user/edit/[slug]'
];

/** @type {import('@sveltejs/kit').Config} */
const config = {
  preprocess: vitePreprocess(),

  kit: {
    adapter: adapterStatic({
      pages: 'build',
      assets: 'build',
      fallback: null,
      precompress: false,
      strict: true
    }),
    prerender: {
      entries: [
        '*', // Include all routes by default
        ...(process.env.NODE_ENV === 'production' ? dynamicRoutesToExclude : [])
      ]
    }
  }
};

export default config;
