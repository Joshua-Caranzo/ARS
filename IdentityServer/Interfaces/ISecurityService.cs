using Identity.API.Dto;
using Identity.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.API.Interfaces
{
    public interface ISecurityService
    {
        public bool ValidatePasswordComplexity(string password);
        //public bool CheckPasswordLength(User user, string newPassword);
        public Task ResetPassword(User user, string password, int webUserId);
        public Task<AjaxCallResultDTO> ValidatePassword(User user, string password1, string password2);
        public Task UpdateUserPassword(User user, string password, byte[] passwordHash, int webUserId);
    }
}
