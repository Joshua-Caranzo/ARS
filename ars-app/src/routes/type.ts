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

export type SchoolDto = 
{
    id:number;
    schoolName:string;
}

export type School = {
    id: number;
    schoolName: string;
    schoolAddress: string;
    schoolAcronym: string;
    schoolContactNum: string;
    schoolEmail: string;
    isActive: boolean;
    imagePath:string | null;
};


export type SchoolYear = {
    id: number;
  fromSchoolTerm: string;
  toSchoolTerm: string;
  isActive: boolean;
};

export type StudentTotal = 
{
    totalMales:number,
    totalFemales:number,
    totalStudents:number
}


export type SectionDto =  {
    gradeLevel:string,
    sectionName:string,
    totalMales:number,
    totalFemales:number,
    totalStudents:number
}

export type LevelDto =  {
    gradeLevel:string,
    totalMales:number,
    totalFemales:number,
    totalStudents:number
}
