import { get, post, postForm, put } from '$lib/api/baseRepo'
import type { CallResultDto } from '../../types/types';
import type { GradeLevel, SchoolSection, SchoolYear, StrandDto, Student } from './type';

export async function getStudentList(searchQuery: string | null, pageNumber: number, pageSize: number, userId: number) {
	return await get<CallResultDto<Student[]>>(`/Student/GetStudentListForEnrollment`,{searchQuery, pageNumber, pageSize, userId});
}

export async function getStudentById( userId: number, fromStudent:boolean) {
	return await get<CallResultDto<Student>>(`/Student/GetStudentById`,{ userId, fromStudent});
}

export async function getGradeLevels() {
	return await get<CallResultDto<GradeLevel[]>>(`/Section/GetGradeLevels`);
}

export async function getStrands() {
	return await get<CallResultDto<StrandDto[]>>(`/Section/GetStrands`);
}

export async function getSchoolYearList() {
	return await get<CallResultDto<SchoolYear[]>>(`/SchoolYear/GetSchoolYearActive`);
}

export async function getSection(id:number, strandId :number | null) {
	return await get<CallResultDto<SchoolSection[]>>(`/Section/GetSchoolSectionbyGrade`,{id,strandId});
}

export async function enrollStudent(userId:number, studentId: number, sectionId:number, syId:number, semId:number|null) {
	return await post<CallResultDto<object>>(`/Student/EnrollStudent`, {
		userId, studentId, sectionId, syId, semId
	});
}
