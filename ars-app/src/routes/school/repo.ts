import { get, post, postForm, put } from '$lib/api/baseRepo'
import type { CallResultDto } from '../../types/types';
import type { School } from './type';

export async function getSchoolList(searchQuery: string | null, pageNumber: number, pageSize: number) {
	return await get<CallResultDto<School[]>>(`/School/GetSchoolList`,{searchQuery, pageNumber, pageSize});
}

export async function getSchoolById(id:number) {
	return await get<CallResultDto<School>>(`/School/GetSchoolById`,{id});
}

export async function addSchool(school: School) {
	return await post<CallResultDto<object>>(`/School/AddSchool`,undefined,school);
}

export async function editSchool(school: School) {
	const formData = new FormData();
    formData.append('Id', String(school.id) || '');
	formData.append('SchoolName', String(school.schoolName) || '');
	formData.append('SchoolAddress', String(school.schoolAddress) || '');
	formData.append('SchoolAcronym', String(school.schoolAcronym) || '');
	formData.append('SchoolContactNum', String(school.schoolContactNum) || '');
	formData.append('SchoolEmail', String(school.schoolEmail) || '');
	formData.append('IsActive', String(school.isActive) || '');
	formData.append('ImagePath', String(school.imagePath) || '');
	return await postForm<CallResultDto<object>>('/School/EditSchool', formData);
}