namespace ARS.API.Models
{
    public class SchoolYear
    {
        public int Id { get; set; }
        public string FromSchoolTerm { get; set; }
        public string ToSchoolTerm { get; set; }
        public bool IsActive { get; set; }
    }
}
