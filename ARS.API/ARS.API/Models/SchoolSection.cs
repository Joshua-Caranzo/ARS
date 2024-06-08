namespace ARS.API.Models
{
    public class SchoolSection
    {
        public int Id { get; set; }
        public int GradeLevelId { get; set; }
        public string SectionName { get; set; }
        public int SchoolId { get; set; }
        public int? StrandId { get; set; }
    }

}
