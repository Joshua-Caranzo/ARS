using System;
using System.ComponentModel.DataAnnotations;

namespace Identity.API.Controllers.Account
{
    public class ForgotPasswordViewModel
    {
        [Required]
        public string Email { get; set; }

        public string Message { get; set; }
    }
}
