using System.ComponentModel.DataAnnotations;

namespace Identity.API.Controllers.Account
{
    public class ChangePasswordViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password1 { get; set; }
        public string Password2 { get; set; }
        public string Uid { get; set; }
        public string Message { get; set; }
    }
}
