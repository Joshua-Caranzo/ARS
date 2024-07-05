import { get, post, postForm, put } from '$lib/api/baseRepo'
import type { CallResultDto } from "../../types/types";
import type { StudentAdminDto, StudentFormData } from './type';

export async function getStudentList(searchQuery: string | null, pageNumber: number, pageSize: number) {
	return await get<CallResultDto<StudentAdminDto[]>>(`/Student/GetStudentListSuperAdmin`,{searchQuery, pageNumber, pageSize});
}

export async function getStudentById(userId: number, fromStudent:boolean) {
	return await get<CallResultDto<StudentFormData>>(`/Student/GetStudentById`,{ userId, fromStudent});
}