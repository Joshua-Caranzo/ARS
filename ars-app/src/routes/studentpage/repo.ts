import { get, post, postForm, put } from '$lib/api/baseRepo'
import type { CallResultDto } from '../../types/types';
import type { EnrollmentHistory, GradeLevel, SchoolSection, SchoolYear, StrandDto, StudentFormData } from './type';

export async function getStudentById( userId: number, fromStudent:boolean) {
	return await get<CallResultDto<StudentFormData>>(`/Student/GetStudentById`,{ userId, fromStudent});
}

export async function getMoveUpStatus( studentId: number) {
	return await get<CallResultDto<boolean>>(`/Student/GetMoveUpStatus`,{ studentId});
}

export async function requestToMoveUp(studentId:number) {
	return await put<CallResultDto<object>>(`/Student/RequestToMoveUp`, {studentId});
}