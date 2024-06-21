import { get, post, put } from '$lib/api/baseRepo'
import type { CallResultDto } from '../../types/types';
import type { School } from '../adminschool/type';
import type { GradeLevel, SchoolSection, SchoolSectionDto, StrandDto } from './type';

export async function getSectionList(searchQuery: string | null, pageNumber: number, pageSize: number, userId:number) {
	return await get<CallResultDto<SchoolSectionDto[]>>(`/Section/GetSectionList`,{searchQuery, pageNumber, pageSize, userId});
}

export async function getGradeLevels() {
	return await get<CallResultDto<GradeLevel[]>>(`/Section/GetGradeLevels`);
}

export async function getStrands() {
	return await get<CallResultDto<StrandDto[]>>(`/Section/GetStrands`);
}

export async function addSection(section: SchoolSection, userId:number) {
	return await post<CallResultDto<object>>(`/Section/AddSection`,{userId},section);
}

export async function getSectionById(id:number) {
	return await get<CallResultDto<SchoolSection>>(`/Section/GetSectionById`,{id});
}

export async function editSection(section: SchoolSection) {
	return await put<CallResultDto<object>>(`/Section/EditSection`,undefined,section);
}

