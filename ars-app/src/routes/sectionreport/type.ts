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