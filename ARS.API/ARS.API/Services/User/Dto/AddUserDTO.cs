namespace Payroll.API.Services.User.Dto
{
    public class AddUserDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int? AssignedSchoolId { get; set; }
        public int UserTypeId { get; set; }
    }
}
