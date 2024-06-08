namespace Payroll.API.Services.User.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public byte[] Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
        public int UserTypeId { get; set; }
        public string UserTypeName { get; set; }
        public string SchoolName { get; set; }
        public int? AssignedSchoolId { get; set; }
    }
}
