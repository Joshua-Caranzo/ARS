import { get, post, put } from '$lib/api/baseRepo'
import type { CallResultDto } from '../../types/types';
import type { AddUserDTO, EditUserDTO, SchoolDto, UserDTO, UserTypeDTO } from './type';

export async function getUserList(searchQuery: string | null, pageNumber: number, pageSize: number) {
	return await get<CallResultDto<UserDTO[]>>(`/User/GetUserList`,{searchQuery, pageNumber, pageSize});
}

export async function getUserById(id: number) {
	return await get<CallResultDto<UserDTO>>(`/User/GetUserById`, {
		id
	});
}

export async function addUser(user: AddUserDTO) {
	return await post<CallResultDto<object>>(`/User/AddUser`,undefined,user);
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