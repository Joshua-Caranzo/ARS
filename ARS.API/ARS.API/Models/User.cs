namespace ARS.API.Models
{
    using System;

    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public byte[] Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
        public Guid? ActivationGuid { get; set; }
        public DateTime? ActivationDueDate { get; set; }
        public string? EncryptedPassword { get; set; }
        public int? NoOfActivationEmailSent { get; set; }
        public int? FailedLogins { get; set; }
        public DateTime? LoginTimeLockout { get; set; }
        public bool? IsLockedOut { get; set; }
        public DateTime? PasswordExpiryDate { get; set; }
        public DateTime? UserCredentialSentDate { get; set; }
        public DateTime? ForgottenPasswordSent { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set;}
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsStudent { get; set; }
        public bool IsEmployee { get; set; }
        public int UserTypeId { get; set; }
        public string Sex { get; set; }
        public string? Suffix { get; set; }
    }

}
