import { get, post, postForm, put } from '$lib/api/baseRepo'
import type { CallResultDto } from '../../types/types';
import type { LevelDto, School, SchoolYear, SectionDto,  StudentTotal } from './type';

export async function getSchoolById(id: number) {
	return await get<CallResultDto<School>>(`/School/GetSchoolByUserId`, {
		id
	});
}

export async function getCurrentSchoolTerm() {
	return await get<CallResultDto<number>>(`/SchoolYear/GetCurrentSchoolTerm`);
}

export async function getSchoolYearList() {
	return await get<CallResultDto<SchoolYear[]>>(`/SchoolYear/GetSchoolYearActive`);
}

export async function getTotalBySection(userId:number, syId:number) {
	return await get<CallResultDto<SectionDto[]>>(`/Section/GetTotalBySection`, {userId, syId});
}

export async function getTotalBySchool( userId:number, syId:number) {
	return await get<CallResultDto<StudentTotal>>(`/Section/GetTotalBySchool`, {userId, syId});
}

export async function getLevelBySection(userId:number, syId:number) {
	return await get<CallResultDto<LevelDto[]>>(`/Section/GetTotalByLevel`, {userId, syId});
}