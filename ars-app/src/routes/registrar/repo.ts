import { get, post, postForm, put } from '$lib/api/baseRepo'
import type { CallResultDto } from '../../types/types';
import type { GradeLevel, StrandDto, StudentFormData } from './type';

export async function addStudent(student:StudentFormData, userId: number, isStudentRegistered:boolean | null) {
	return await post<CallResultDto<object>>(`/Student/AddStudent`, {
		userId, isStudentRegistered
	}, student);
}

export async function getStudentList(searchQuery: string | null, pageNumber: number, pageSize: number, userId: number) {
	return await get<CallResultDto<StudentFormData[]>>(`/Student/GetStudentList`,{searchQuery, pageNumber, pageSize, userId});
}

export async function getStudentById( userId: number, fromStudent:boolean) {
	return await get<CallResultDto<StudentFormData>>(`/Student/GetStudentById`,{ userId, fromStudent});
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

export async function denyStudent(studentId:number) {
	return await put<CallResultDto<object>>(`/Student/DenyStudent`,{studentId});
}
