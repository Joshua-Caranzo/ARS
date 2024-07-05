import { get, post, postForm, put } from '$lib/api/baseRepo'
import type { CallResultDto } from '../types/types';
import type { GradeLevel, LevelDto, School, SchoolDto, SchoolYear, SectionDto, StrandDto, StudentFormData, StudentTotal } from './type';

export async function addStudent(student:StudentFormData, userId: number, isStudentRegistered:boolean | null) {
	return await post<CallResultDto<object>>(`/Student/AddStudent`, {
		userId, isStudentRegistered
	}, student);
}

export async function getStudentList(searchQuery: string | null, pageNumber: number, pageSize: number, userId: number) {
	return await get<CallResultDto<StudentFormData[]>>(`/Student/GetStudentList`,{searchQuery, pageNumber, pageSize, userId});
}

export async function getStudentById( userId: number) {
	return await get<CallResultDto<StudentFormData>>(`/Student/GetStudentById`,{ userId});
}

export async function editStudent(student:StudentFormData, userId: number) {
	return await put<CallResultDto<object>>(`/Student/EditStudent`, {
		userId
	}, student);
}

export async function getGradeLevels() {
	return await get<CallResultDto<GradeLevel[]>>(`/Section/GetGradeLevels`);
}

export async function getStrands() {
	return await get<CallResultDto<StrandDto[]>>(`/Section/GetStrands`);
}

export async function getSchool() {
	return await get<CallResultDto<SchoolDto[]>>(`/User/GetSchool`);
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

export async function getTotalByLevel(userId:number, syId:number) {
	return await get<CallResultDto<SectionDto[]>>(`/Section/GetTotalByLevel`, {userId, syId});
}

export async function getTotalBySchool( userId:number, syId:number) {
	return await get<CallResultDto<StudentTotal>>(`/Section/GetTotalBySchool`, {userId, syId});
}

export async function GetTotalBySchoolSuperAdmin( schoolId:number, syId:number) {
	return await get<CallResultDto<StudentTotal>>(`/Section/GetTotalBySchoolSuperAdmin`, {schoolId, syId});
}

export async function getLevelBySection(schoolId:number, syId:number) {
	return await get<CallResultDto<LevelDto[]>>(`/Section/GetTotalByLevelSuperAdmin`, {schoolId, syId});
}

export async function getAllSchools() {
	return await get<CallResultDto<School[]>>(`/School/GetAllSchools`);
}