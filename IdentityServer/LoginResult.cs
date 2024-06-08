using System;

namespace Identity.API
{
    public class LoginResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public bool IsLockedOut { get; set; }
        public int FailedLoginCount { get; set; }
        public DateTime LoginTimeLockout { get; set; }
        public LoginResult()
        {
        }
        public LoginResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}
