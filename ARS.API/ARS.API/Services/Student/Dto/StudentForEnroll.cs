namespace ARS.API.Services.Student.Dto
{
    public class StudentForEnroll
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string? ContactNumber { get; set; }
        public string LRN { get; set; }
        public string StudentIdNumber { get; set; }
        public string GradeLevel { get; set; }
        public string? StrandName { get; set; }
        public int? StrandId { get; set; }
        public int GradeLevelId { get; set; }
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
        public string? MotherContactNumber { get; set; }
        public string? FatherContactNumber { get; set; }
        public string? GuardianContactNumber { get; set; }
        public string? MotherEmailAddress { get; set; }
        public string? FatherEmailAddress { get; set; }
        public string? GuardianEmailAddress { get; set; }
        public string Sex { get; set; }
        public bool IsEnrolled { get; set; }
    }

}
