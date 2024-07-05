import { get, post, postForm, put } from '$lib/api/baseRepo'
import type { CallResultDto } from '../../types/types';
import type { EnrollmentHistory, GradeLevel, SchoolSection, SchoolYear, StrandDto, Student } from './type';

export async function getStudentById( userId: number, syId:number|null) {
	return await get<CallResultDto<Student>>(`/Student/GetStudentProfile`,{ userId, syId});
}

export async function getMoveUpStatus( studentId: number) {
	return await get<CallResultDto<boolean>>(`/Student/GetMoveUpStatus`,{ studentId});
}

export async function requestToMoveUp(studentId:number) {
	return await put<CallResultDto<object>>(`/Student/RequestToMoveUp`, {studentId});
}

export async function getCurrentSchoolTerm() {
	return await get<CallResultDto<number>>(`/SchoolYear/GetCurrentSchoolTerm`);
}