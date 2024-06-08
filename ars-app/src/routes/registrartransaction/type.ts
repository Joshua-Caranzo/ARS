export type  Student = {
    id: number;
    lastName: string;
    middleName: string | null; // Assuming middleName can be null if not available
    firstName: string;
    email: string;
    contactNumber: string;
    lrn: string;
    studentIdNumber: string;
    lastSchoolAttendedYear: number; // Assuming this is a year
    gradeLevel: string; // Assuming gradeLevel is a string, change to number if needed
    strandName: string;
    gradeLevelId:number;
    strandId : number | null;
    isEnrolled:boolean;
}
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