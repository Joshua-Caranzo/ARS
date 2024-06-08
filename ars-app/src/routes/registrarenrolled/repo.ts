import { get, post, postForm, put } from '$lib/api/baseRepo'
import type { CallResultDto } from '../../types/types';
import type { EnrollmentHistory, GradeLevel, SchoolSection, SchoolYear, StrandDto, Student } from './type';

export async function getStudentList(searchQuery: string | null, pageNumber: number, pageSize: number, userId: number, syId:number) {
	return await get<CallResultDto<Student[]>>(`/Student/GetStudentListEnrolled`,{searchQuery, pageNumber, pageSize, userId, syId});
}

export async function getStudentById( studentId: number) {
	return await get<CallResultDto<EnrollmentHistory[]>>(`/Student/GetEnrollmentHistory`,{ studentId});
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

export async function getSection(id:number, strandId:number|null) {
	return await get<CallResultDto<SchoolSection[]>>(`/Section/GetSchoolSectionbyGrade`,{id,strandId});
}

export async function enrollStudent(userId:number, studentId: number, sectionId:number, syId:number, semId:number|null, strandId:number|null) {
	return await post<CallResultDto<object>>(`/Student/EnrollStudent`, {
		userId, studentId, sectionId, syId, semId,strandId
	});
}

export async function getCurrentSchoolTerm() {
	return await get<CallResultDto<number>>(`/SchoolYear/GetCurrentSchoolTerm`);
}