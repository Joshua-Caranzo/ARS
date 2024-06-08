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
export type Student = 
{
    lastName:string;
    firstName:string;
    middleName:string | null;
    sex:string;
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