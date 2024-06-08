namespace ARS.API.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string StudentIdNumber { get; set; }
        public string? ContactNumber { get; set; }
        public string? LRN { get; set; }
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
        public string BirthPlace { get; set; }
        public string CivilStatus { get; set; }
        public string Religion { get; set; }
        public string? MotherName { get; set; }
        public string? MotherAddress { get; set; }
        public string? FatherName { get; set; }
        public string? FatherAddress { get; set; }
        public string GuardianName { get; set; }
        public string? GuardianAddress { get; set; }
        public string? LastSchoolAttended { get; set; }
        public string? LastSchoolAttendedYear { get; set; }
        public bool IsRegistered { get; set; }
        public bool IsEnrolled { get; set; }
        public bool IsForConfirmation { get; set; }
        public bool IsActive { get; set; }
        public bool IsGraduated { get; set; }
        public int UserId { get; set; }
        public int SchoolId { get; set; }
        public int GradeLevelId { get; set; }
        public int? StrandId { get; set; }
        public string? MotherContactNumber { get; set; }
        public string? FatherContactNumber { get; set; }
        public string? GuardianContactNumber { get; set; }
        public string? MotherEmailAddress { get; set; }
        public string? FatherEmailAddress { get; set; }
        public string? GuardianEmailAddress { get; set; }
        public string? MotherOccupation { get; set; }
        public string? FatherOccupation { get;set; }
        public string GuardianRelationship { get; set; }
        public bool? IsMotherDeceased { get; set; }
        public bool? IsFatherDeceased { get; set; }
        public DateTime? DateRegistered { get; set; }
        public bool RequestToMoveUp { get; set; }
    }
}
