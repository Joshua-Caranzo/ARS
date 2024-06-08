namespace Payroll.API.Services.User.Dto
{
    public class EditUserDTO
    {
        public int Id { get; set; } 
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
        public int UserTypeId { get; set; }
        public int AssignedSchoolId { get; set; }
    }
}
