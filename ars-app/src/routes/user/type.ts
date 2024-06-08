export type UserDTO =  {
    id: number;
    userName: string;
    password: Uint8Array;
    firstName: string;
    lastName: string;
    email: string;
    active: boolean;
    loginTimeLockout?: Date;
    isLockedOut: boolean;
    passwordExpiryDate?: Date;
    userCredentialSentDate?: Date;
    forgottenPasswordSent?: Date;
    activationDueDate?: Date;
    userTypeName: string;
    userTypeId:number;
    schoolName: string;
    assignedSchoolId:number;
}

export type AddUserDTO =  {
    userName: string;
    password: string;
    firstName: string;
    lastName: string;
    email: string;
    userTypeId:number;
    assignedSchoolId:number;
}

export type EditUserDTO =  {
    id:number;
    userName: string;
    firstName: string;
    lastName: string;
    email: string;
    active: boolean;
    userTypeId : number;
    assignedSchoolId : number;
}

export type UserTypeDTO = 
{
    id:number;
    typeName:string;
}

export type SchoolDto = 
{
    id:number;
    schoolName:string;
}