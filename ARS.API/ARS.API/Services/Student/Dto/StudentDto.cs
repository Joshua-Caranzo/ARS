namespace ARS.API.Services.Student.Dto
{
    public class StudentDto
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string Email { get; set; }
        public string? ContactNumber { get; set; }
        public string? LRN { get; set; }
        public DateTime Birthdate { get; set; }
        public string Birthplace { get; set; }
        public string CivilStatus { get; set; }
        public string Religion { get; set; }
        public string Sex { get; set; }
        public string? MothersName { get; set; }
        public string? MothersAddress { get; set; }
        public string? FathersName { get; set; }
        public string? FathersAddress { get; set; }
        public string GuardiansName { get; set; }
        public string GuardiansAddress { get; set; }
        public string? LastSchoolAttended { get; set; }
        public string? LastSchoolAttendedYear { get; set; }
        public int GradeLevelId { get; set; }
        public int? StrandId { get; set; }
        public string? MothersContactNumber { get; set; }
        public string? FathersContactNumber { get; set; }
        public string? GuardiansContactNumber { get; set; }
        public string? MothersEmailAddress { get; set; }
        public string? FathersEmailAddress { get; set; }
        public string? GuardiansEmailAddress { get; set; }
        public string? MotherOccupation {  get; set; }
        public string? FatherOccupation { get; set; }
        public bool? IsMotherDeceased { get; set; }
        public bool? IsFatherDeceased { get; set; }
        public string GuardianRelationship { get; set; }
        public string? Suffix { get; set; }
        public string? Age { get; set; }
        public string? StudentIdNumber { get; set; }
    }
}
