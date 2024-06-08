namespace ARS.API.Services.Section.Dto
{
    public class StudentMasterlist
    {
        public string StudentIdNumber { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string Sex { get; set; }
        public DateTime BirthDate { get; set; }
        public string? Lrn { get; set; }
        public string ContactNumber { get; set; }
        public string Religion { get; set; }
        public string? FatherName { get; set; }
        public string? FatherOccupation { get; set; }
        public string? MotherName { get; set; }
        public string? MotherOccupation { get; set; }
    }
}
