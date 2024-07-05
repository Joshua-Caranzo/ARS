export type Student = {
    id: number;
    lastName: string;
    middleName: string | null; // Assuming middleName can be null if not available
    firstName: string;
    email: string;
    contactNumber: string;
    lrn: string;
    studentIdNumber: string;
    gradeLevel: string; // Assuming gradeLevel is a string, change to number if needed
    strandName: string;
    gradeLevelId: number;
    birthDate: Date;
    age: number;
    birthPlace: string;
    civilStatus: string;
    religion: string;
    motherName: string | null;
    motherAddress: string | null;
    fatherName: string | null;
    fatherAddress: string | null;
    guardianName: string;
    guardianAddress: string | null;
    lastSchoolAttended: string | null;
    lastSchoolAttendedYear: string | null; // Assuming this is a year
    motherContactNumber: string | null;
    fatherContactNumber: string | null;
    guardianContactNumber: string | null;
    motherEmailAddress: string | null;
    fatherEmailAddress: string | null;
    guardianEmailAddress: string | null;
    sex: string;
    gradeLevelForSy:string;
    studentAddress:string,
    isLockedOut:boolean
};

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


export type SchoolSection =  {
    id: number;
    gradeLevelId: number;
    sectionName: string;
    schoolId: number;
    strandId?: number | null;
}

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

export type SchoolYear = {
    id: number;
  fromSchoolTerm: string;
  toSchoolTerm: string;
  isActive: boolean;
};

export type EnrollmentHistory = 
{
    id:number,
    dateEnrolled:Date,
    level:string,
    sectionName:string
}

export type CurrentSection = 
{
    sectionId:number;
    gradeLevelId:number;
    sectionName:string;
}

export type CurrentStrand = 
{
    strandId:number;
    gradeLevelId:number;
}