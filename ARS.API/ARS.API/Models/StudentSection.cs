namespace ARS.API.Models
{
    public class StudentSection
    {
        public int Id { get; set; }
        public int? SectionId { get; set; }
        public int StudentId { get; set; }
        public bool IsActive { get; set; }
    }
}
