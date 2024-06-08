namespace ARS.API.Models
{
    public class School
    {
        public int Id { get; set; }
        public string SchoolName { get; set; }
        public string SchoolAddress { get; set; }
        public string SchoolAcronym { get; set; }
        public string SchoolContactNum { get; set; }
        public string SchoolEmail { get; set; }
        public bool IsActive { get; set; }
        public string? ImagePath { get; set; }
    }
}
