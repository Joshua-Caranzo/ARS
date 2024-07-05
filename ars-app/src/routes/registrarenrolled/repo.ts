import { get, post, postForm, put } from '$lib/api/baseRepo'
import type { CallResultDto } from '../../types/types';
import type { StudentFormData } from '../type';
import type { CurrentSection, CurrentStrand, EnrollmentHistory, GradeLevel, SchoolSection, SchoolYear, StrandDto, Student } from './type';

export async function getStudentList(searchQuery: string | null, pageNumber: number, pageSize: number, userId: number, syId:number) {
	return await get<CallResultDto<Student[]>>(`/Student/GetStudentListEnrolled`,{searchQuery, pageNumber, pageSize, userId, syId});
}

export async function getStudentById( studentId: number) {
	return await get<CallResultDto<EnrollmentHistory[]>>(`/Student/GetEnrollmentHistory`,{ studentId});
}

export async function getStudentDetailsById( userId: number, fromStudent:boolean) {
	return await get<CallResultDto<StudentFormData>>(`/Student/GetStudentById`,{ userId, fromStudent});
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

export async function getSection(userId:number, id:number, strandId:number|null) {
	return await get<CallResultDto<SchoolSection[]>>(`/Section/GetSchoolSectionbyGrade`,{userId, id,strandId});
}

export async function enrollStudent(userId:number, studentId: number, sectionId:number, syId:number, semId:number|null, strandId:number|null) {
	return await post<CallResultDto<object>>(`/Student/EnrollStudent`, {
		userId, studentId, sectionId, syId, semId,strandId
	});
}

export async function getCurrentSchoolTerm() {
	return await get<CallResultDto<number>>(`/SchoolYear/GetCurrentSchoolTerm`);
}

export async function getNotes(studentId:number) {
	return await get<CallResultDto<string>>(`/Student/GetNotes`,{studentId});
}

export async function addNotes(studentId:number, notes:string) {
	return await put<CallResultDto<object>>(`/Student/AddNotes`,{studentId, notes});
}

export async function getCurrentSection(studentId:number, syId: number) {
	return await get<CallResultDto<CurrentSection>>(`/Section/GetCurrentSection`,{studentId, syId});
}

export async function changeSection(studentId:number, syId:number, sectionId:number) {
	return await put<CallResultDto<object>>(`/Section/ChangeSection`,{studentId, syId, sectionId});
}

export async function getCurrentStrand(studentId:number, syId: number) {
	return await get<CallResultDto<CurrentStrand>>(`/Section/GetCurrentStrand`,{studentId, syId});
}

export async function changeStrand(studentId:number, sectionId:number, strandId:number, syId:number) {
	return await put<CallResultDto<object>>(`/Section/ChangeStrand`,{studentId,sectionId,strandId,  syId});
}