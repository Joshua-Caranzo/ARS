using System.ComponentModel.DataAnnotations;

namespace Identity.API.Controllers.Account
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "The password must be at least 8 characters long.")]
        [RegularExpression(@"(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&#]).{8,}", ErrorMessage = "The password must contain at least one uppercase letter, one lowercase letter, one number, and one symbol.")]
        public string Password1 { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password1", ErrorMessage = "The password and confirmation password do not match.")]
        public string Password2 { get; set; }

        public string Message { get; set; }
    }
}
