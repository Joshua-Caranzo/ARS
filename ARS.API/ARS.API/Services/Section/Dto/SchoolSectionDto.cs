namespace ARS.API.Services.Section.Dto
{
    public class SchoolSectionDto
    {
        public int Id { get; set; }
        public int GradeLevelId { get; set; }
        public string SectionName { get; set; }
        public int SchoolId { get; set; }
        public int? StrandId { get; set; }
        public string Level { get; set; }
        public string? StrandName { get; set; }
    }
}
