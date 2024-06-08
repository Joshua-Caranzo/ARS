import { get, post, postForm, put } from '$lib/api/baseRepo'
import type { CallResultDto } from '../../types/types';
import type {  SchoolYear } from './type';

export async function getSchoolYearList(searchQuery: string | null, pageNumber: number, pageSize: number) {
	return await get<CallResultDto<SchoolYear[]>>(`/SchoolYear/GetSchoolYearList`,{searchQuery, pageNumber, pageSize});
}

export async function getSchoolYearById(id:number) {
	return await get<CallResultDto<SchoolYear>>(`/SchoolYear/GetSchoolYearById`,{id});
}

export async function addSchoolYear(schoolYear: SchoolYear) {
	return await post<CallResultDto<object>>(`/SchoolYear/AddSchoolYear`,undefined,schoolYear);
}

export async function editSchoolYear(schoolYear: SchoolYear) {
	return await put<CallResultDto<object>>('/SchoolYear/EditSchoolYear', undefined, schoolYear);
}