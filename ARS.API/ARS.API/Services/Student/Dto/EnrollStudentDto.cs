namespace ARS.API.Services.Student.Dto
{
    public class EnrollStudentDto
    {
        public int ID { get; set; }
        public DateTime DateEnrolled { get; set; }
        public string SectionName { get; set; }
        public string Level { get; set; }
    }
}
