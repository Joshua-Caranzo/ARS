using Identity.API.Data;
using Identity.API.Entities;
using Identity.API.Helper;
using Identity.API.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.API.Services
{
    public class UserService : IUserService
    {
        private readonly IdentityContext _dbContext;
        private readonly IWebHostEnvironment _hostingEnvironment;


        public UserService(IdentityContext dbContext, IWebHostEnvironment hostingEnvironment)
        {
            _dbContext = dbContext;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<User> GetById(int id)
        {
            return await _dbContext.User.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByUserNameAsync(string username)
        {
            return await _dbContext.User.FirstOrDefaultAsync(u => u.UserName == username && u.Active == true);
        }

        public async Task UnlockUserAccount(int id)
        {
            var user = await _dbContext.User.Where(wr => wr.Id == id).SingleOrDefaultAsync();
            user.FailedLogins = null;
            user.LoginTimeLockout = null;
            user.IsLockedOut = false;
            await _dbContext.SaveChangesAsync();
        }

        public async Task UnlockUserAccount(string userName)
        {
            var user = await _dbContext.User.Where(wr => wr.UserName == userName).FirstOrDefaultAsync();
            if (user != null)
            {
                user.FailedLogins = null;
                user.LoginTimeLockout = null;
                user.IsLockedOut = false;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> IsLockedOut(int id)
        {
            var user = await _dbContext.User.Where(wr => wr.Id == id).SingleOrDefaultAsync();
            if (!(bool)user.IsLockedOut)
            {
                return (bool)user.IsLockedOut;
            }
            var loginTimeLockout = (user.LoginTimeLockout != null) ? Convert.ToDateTime(user.LoginTimeLockout) : DateTime.MinValue;
            if (DateTime.Now > loginTimeLockout)
            {
                await UnlockUserAccount(user.Id);
                return false;
            }
            return (bool)user.IsLockedOut;
        }

        public async Task<bool> IsLockedOut(string username)
        {
            var loginResult = new LoginResult();
            var user = await _dbContext.User.FirstOrDefaultAsync(wr => wr.UserName == username);
            if (user != null)
            {
                loginResult.LoginTimeLockout = (user.LoginTimeLockout != null) ? Convert.ToDateTime(user.LoginTimeLockout) : DateTime.MinValue;
                loginResult.IsLockedOut = Convert.ToBoolean(user.IsLockedOut);

                if (!loginResult.IsLockedOut)
                {
                    return loginResult.IsLockedOut; // always false here...
                }

                TimeSpan diff = DateTime.Now - loginResult.LoginTimeLockout;

                if (diff.Days >= 1)
                {
                    if (DateTime.Now > loginResult.LoginTimeLockout)
                    {
                        await UnlockUserAccount(username);
                        return false;
                    }
                }
                return loginResult.IsLockedOut; // always true here...
            }

            return loginResult.IsLockedOut;
        }

        public async Task<string> GetValidUser(string userName, byte[] password, bool executeLogin)
        {
            var result = string.Empty;
            var user = await _dbContext.User.Where(wr => wr.UserName == userName && wr.Password == password && wr.Active == true).SingleOrDefaultAsync();
            return result;
        }

        public async Task<string> GetValidUser(string userName, bool executeLogin)
        {
            var result = string.Empty;
            var user = await _dbContext.User.Where(wr => wr.UserName == userName && wr.Active == true).SingleOrDefaultAsync();
            return result;
        }
        public async Task<int> GetNumFailedLogins(string username)
        {
            var user = await _dbContext.User.Where(wr => wr.UserName == username).FirstOrDefaultAsync();
            if (user != null)
            {
                return user.FailedLogins.GetValueOrDefault();
            }
            return 0;
        }

        public async Task IncrementFailedLogins(string userName, int failedLogins)
        {
            var user = await _dbContext.User.Where(wr => wr.UserName == userName).FirstOrDefaultAsync();
            if (user != null)
            {


                user.FailedLogins = failedLogins;
                user.LoginTimeLockout = DateTime.Now.AddMinutes(StringUtilities.GetPasswordAttemptWindow());
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task LockUserAccount(string username)
        {
            var user = await _dbContext.User.Where(wr => wr.UserName == username).FirstOrDefaultAsync();
            if (user != null)
            {
                user.IsLockedOut = true;
                user.LoginTimeLockout = DateTime.Now.AddMinutes(StringUtilities.GetPasswordAttemptWindow());
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> FailedLoginAttempt(string userName)
        {

            var numFailedLogins = await GetNumFailedLogins(userName);

            if (numFailedLogins < 5)//todo: add in apsettings maxlogin allowed
            {
                // increment, update loginTimeLockout and return false
                await IncrementFailedLogins(userName, ++numFailedLogins);
                return false;
            }
            else
            {
                // update loginTimeLockout, update IsLockedOut and return true
                await LockUserAccount(userName);
                return true;
            }
        }

        public async Task<LoginResult> Login(string userName, string password)
        {
            // Check login is locked by username
            if (await IsLockedOut(userName))
            {
                return new LoginResult(false, "Your account has been locked out due to too many failed login attempts. Please contact System Administrator!");
            }

            // Attempt to find the user with the given username
            var user = await _dbContext.User
                                       .Where(wr => wr.UserName == userName && wr.Active == true)
                                       .SingleOrDefaultAsync();

            //if not found check using lower case
            if (user == null) 
            {
                var userNameLower = userName.ToLower();
                user = await _dbContext.User.Where(wr => wr.UserName.ToLower() == userName.ToLower() && wr.Active == true).SingleOrDefaultAsync();
            }

            //if failed checking using lower case, return false
            if (user == null)
            {
                return new LoginResult(false, "Invalid username or password.");
            }

            // Prevents throwing of exception if EncyptedPassword is null
            // Most user in the database have null EncryptedPassword 
            if (string.IsNullOrEmpty(user.EncryptedPassword))
            {
                return new LoginResult(false, "Invalid username or password.");
            }


            // Verify the password using BCrypt instead of MD5
            if (!BCrypt.Net.BCrypt.Verify(password, user.EncryptedPassword))
            {
                // Handle failed login attempt (e.g., increment failed login counter, possibly lock the account)
                // await HandleFailedLoginAttempt(user);
                return new LoginResult(false, "Invalid username or password.");
            }

            // If login is successful, unlock the user account if it was locked
            await UnlockUserAccount(user.Id);
            return new LoginResult(true, "");
        }

        public async Task UpdateUserForgottenPasswordSent(int id)
        {
            var user = await _dbContext.User.Where(wr => wr.Id == id).SingleOrDefaultAsync();
            if (user != null)
            {
                user.ForgottenPasswordSent = DateTime.Now;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task UpdateUserActivationDueDateDays(int id, int? activationDueDays)
        {
            var user = await _dbContext.User.Where(wr => wr.Id == id).SingleOrDefaultAsync();
            user.ActivationDueDate = DateTime.Now.AddDays((int)activationDueDays);
            user.NoOfActivationEmailSent = 0;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> CheckVerificationLockout(string userName)
        {
            bool isLockedOut = await FailedLoginAttempt(userName);
            if (isLockedOut)
            {

                var emailMessagePath = _hostingEnvironment.ContentRootPath + "\\template\\user-lockout.html";
                if (!string.IsNullOrEmpty(emailMessagePath))
                {
                    var emailMessage = System.IO.File.ReadAllText(emailMessagePath);

                    emailMessage = emailMessage.Replace("%%USERNAME%%", userName).Replace("%%TIME%%", DateTime.Now.ToString("dddd, dd MMMM yyyy hh:mm tt"));

                    EmailUtilities.SendEmail("helpdesk@if-audit.com", emailMessage, "User Account Locked!");

                }
            }
            return isLockedOut;
        }
    }
}
