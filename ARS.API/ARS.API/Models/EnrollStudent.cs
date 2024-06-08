namespace ARS.API.Models
{
    public class EnrollStudent
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public DateTime DateEnrolled { get; set; }
        public int? SectionId { get; set; }
        public int SchoolId { get; set; }
        public int SchoolYearId { get; set; }
        public int? SemesterId { get; set; }
    }
}
