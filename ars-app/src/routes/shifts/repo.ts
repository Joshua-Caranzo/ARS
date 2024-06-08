import { get, post, put } from "$lib/api/baseRepo";
import type { CallResultDto } from "../../types/types";
import type { ShiftsDto, TimePolicyCode } from "./type";

export async function getShiftList() {
	return await get<CallResultDto<ShiftsDto[]>>(`/Shifts/GetShiftList`);
}

export async function getShiftListById(id: number) {
	return await get<CallResultDto<ShiftsDto>>(`/Shifts/GetShiftListById`,{
		id
	});
}

export async function getTimePolicyCode() {
	return await get<CallResultDto<TimePolicyCode[]>>(`/Shifts/GetTimePolicyCode`);
}

export async function addShift(shift: ShiftsDto) {
	return await post<CallResultDto<object>>(`/Shifts/AddShift`,undefined,shift);
}

export async function editShift(shift: ShiftsDto) {
	return await put<CallResultDto<object>>(`/Shifts/EditShift`,undefined,shift);
}