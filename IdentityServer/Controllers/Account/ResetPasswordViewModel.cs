using System.ComponentModel.DataAnnotations;

namespace Identity.API.Controllers.Account
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string Password1 { get; set; }

        public string Password2 { get; set; }

        public string Message { get; set; }
    }
}
