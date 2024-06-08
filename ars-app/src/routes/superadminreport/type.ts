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

export type GradeLevel = 
{
    id:number,
    level:string,
    departmentId:number
}
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

export type StudentInfo = {
    studentIdNumber: string;
    lastName: string;
    firstName: string;
    middleName: string | null;
    sex: string;
    birthDate: Date;
    lrn: string | null;
    contactNumber: string;
    religion: string;
    fatherName: string | null;
    fatherOccupation: string | null;
    motherName: string | null;
    motherOccupation: string | null;
}


export type SchoolSectionDto =  {
    id: number;
    gradeLevelId: number;
    sectionName: string;
    schoolId: number;
    strandId?: number | null;
    level: string;
    strandName: string | null;
}

export type StrandDto =  {
    id: number;
    strandName: string;
    strandAbbrev?: string | null;
    trackId: number;
    trackName: string;
}

