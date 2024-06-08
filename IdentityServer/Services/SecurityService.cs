using Identity.API.Data;
using Identity.API.Dto;
using Identity.API.Entities;
using Identity.API.Helper;
using Identity.API.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Identity.API.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly IdentityContext _dbContext;
        private readonly IUserService _userService;
        public SecurityService(IdentityContext dbContext, IUserService userService)
        {
            _dbContext = dbContext;
            _userService = userService;
        }

        public bool ValidatePasswordComplexity(string password)
        {
            var passwordFormat = @"^(?=(.*\d))(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z\d]).{8,}$";
            Regex regex = new Regex(passwordFormat);
            return regex.Match(password).Success;
        }

        //public bool CheckPasswordLength(User user, string newPassword)
        //{
        //    if (user.PasswordLength != null && (newPassword.Length < user.PasswordLength))
        //        return false;

        //    return true;
        //}

        public async Task ResetPassword(User user, string password, int webUserId)
        {
            byte[] hashedPassword = StringUtilities.GetMd5Hash(password, user.UserName);
            await UpdateUserPassword(user.Id, hashedPassword, webUserId);
            byte[] buffer = StringUtilities.GetMd5Hash(password, user.UserName.ToLower());
            await UpdateUserPasswordExpiryDate(user.Id, 180, webUserId);
        }

        public async Task UpdateUserPassword(User user, string password, byte[] passwordHash, int webUserId)
        {
            await UpdateUserPassword(user.Id, passwordHash, webUserId);
            byte[] buffer = StringUtilities.GetMd5Hash(password, user.UserName.ToLower());
            await UpdateUserPasswordExpiryDate(user.Id, 180, webUserId);
        }

        private async Task UpdateUserPassword(int userId, byte[] password, int webUserId)
        {
            var user = await _dbContext.User.Where(x => x.Id == userId).SingleOrDefaultAsync();
            user.Password = password;
            user.UpdatedDate = DateTime.Now;
            await _dbContext.SaveChangesAsync();
        }

        //private async Task UpdateUserLowerLogin(int id, string lowerCaseUserName, byte[] lowerCasePassword)
        //{
        //    var user = await _dbContext.User.Where(x => x.Id == id).SingleOrDefaultAsync();
        //    user.UserNameLower = lowerCaseUserName;
        //    user.PasswordLower = lowerCasePassword;
        //    await _dbContext.SaveChangesAsync();
        //}

        private async Task UpdateUserPasswordExpiryDate(int userId, int days, int webUserId)
        {
            var user = await _dbContext.User.Where(x => x.Id == userId).SingleOrDefaultAsync();
            user.PasswordExpiryDate = DateTime.Now.AddDays(days);
            user.UpdatedDate = DateTime.Now;
            await _dbContext.SaveChangesAsync();
        }


        public async Task<AjaxCallResultDTO> ValidatePassword(User user, string password1, string password2)
        {
            var result = new AjaxCallResultDTO();
            result.Success = true;

            if (string.IsNullOrWhiteSpace(password1))
            {
                result.Message += "Password is mandatory. ";
                result.Success = false;
            }
            if (string.IsNullOrWhiteSpace(password2))
            {
                result.Message += "Repeat Password is mandatory. ";
                result.Success = false;
            }
            if (password1 != password2)
            {
                result.Message += "Passwords must match. ";
                result.Success = false;
            }

            //if (!CheckPasswordLength(user, password1))
            //{
            //    result.Message += "Password length must be equal or greater than " + user.PasswordLength.ToString() + ".";
            //    result.Success = false;
            //}

            if (!ValidatePasswordComplexity(password1))
            {
                result.Message += "Password does not meet complexity rules. ";
                result.Success = false;
            }

            return result;
        }
    }
}
