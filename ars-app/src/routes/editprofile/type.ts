export type StudentFormData = {
    id: number;
    lastName: string;
    firstName: string;
    suffix:string |  null;
    middleName?: string | null;
    email: string;
    contactNumber?: string | null;
    lrn: string | null;
    birthdate: Date; 
    birthplace: string;
    civilStatus: string;
    religion: string;
    sex: string;
    mothersName?: string | null;
    mothersAddress?: string | null;
    mothersContactNumber?: string | null;
    mothersEmailAddress?: string | null;
    fathersName?: string | null;
    fathersAddress?: string | null;
    fathersContactNumber?: string | null;
    fathersEmailAddress?: string | null;
    guardiansName: string;
    guardiansAddress: string;
    guardiansContactNumber?: string | null;
    guardiansEmailAddress?: string | null;
    lastSchoolAttended: string | null;
    lastSchoolAttendedYear: string | null; 
    gradeLevelId: number;
    strandId: number | null;
    isMotherDeceased:boolean | null;
    isFatherDeceased:boolean|null;
    motherOccupation : string|null;
    fatherOccupation:string|null;
    guardianRelationship:string;
    studentAddress:string;
};

export type StrandDto =  {
    id: number;
    strandName: string;
    strandAbbrev?: string | null;
    trackId: number;
    trackName: string;
}

export type GradeLevel = 
{
    id:number,
    level:string,
    departmentId:number
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