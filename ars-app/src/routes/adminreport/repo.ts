import { get, post, postForm, put } from '$lib/api/baseRepo'
import type { CallResultDto } from '../../types/types';
import type { School, SchoolSectionDto, SchoolYear, Student } from './type';

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

export async function getSchoolReport(userId:number, gradeLevelId:number, syId:number) {
	return await get<CallResultDto<Student[]>>(`/Student/GetStudentReport`, {userId, gradeLevelId, syId});
}

export async function getSectionList( userId:number) {
	return await get<CallResultDto<SchoolSectionDto[]>>(`/Section/GetSchoolGradeLevels`, {userId});
}