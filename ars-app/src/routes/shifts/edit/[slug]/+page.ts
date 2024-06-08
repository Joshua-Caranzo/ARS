export const load = async ({ params }) => {
	
    let id = 0;

    try {
        id = parseInt(params.slug);
        if (isNaN(id)) {
            id = 0;
        }
    } catch (error) {
        id = 0;
    }    

	return {
        id
	};
};