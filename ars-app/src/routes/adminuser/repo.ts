import { get, post, put } from '$lib/api/baseRepo'
import type { CallResultDto } from '../../types/types';
import type { AddUserDTO, EditUserDTO, SchoolDto, UserDTO, UserTypeDTO } from './type';

export async function getUserList(searchQuery: string | null, pageNumber: number, pageSize: number, userId:number) {
	return await get<CallResultDto<UserDTO[]>>(`/User/GetUserListAdmin`,{searchQuery, pageNumber, pageSize, userId});
}

export async function getUserById(id: number) {
	return await get<CallResultDto<UserDTO>>(`/User/GetUserById`, {
		id
	});
}

export async function addUserAdmin(user: AddUserDTO, userId:number) {
	console.log(userId);
	console.log(user);
	return await post<CallResultDto<object>>(`/User/AddUserAdmin`, {userId}, user);
}

export async function editUser(user: EditUserDTO) {
	return await put<CallResultDto<object>>(`/User/EditUser`,undefined,user);
}

export async function getUserType() {
	return await get<CallResultDto<UserTypeDTO[]>>(`/User/GetUserType`);
}

export async function getSchool() {
	return await get<CallResultDto<SchoolDto[]>>(`/User/GetSchool`);
}