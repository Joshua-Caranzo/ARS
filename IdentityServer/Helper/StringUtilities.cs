using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Identity.API.Helper
{
  
    public sealed class StringUtilities
    {
        public static IConfigurationRoot _root;
        private readonly IWebHostEnvironment _environment;
        static StringUtilities()
        {
            var configurationBuilder = new ConfigurationBuilder();
            string path = string.Empty;
            bool isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
            if (isDevelopment)
            {
                path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Development.json");
            }
            else
            {
                path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            }
          

            configurationBuilder.AddJsonFile(path, false);
            _root = configurationBuilder.Build();
        }
        public static byte[] GetMd5Hash(string value, string salt)
        {
            var md5Hasher = new MD5CryptoServiceProvider();
            var encoder = new UTF8Encoding();
            return md5Hasher.ComputeHash(encoder.GetBytes(value + salt));
        }

        public static bool GetValidateLoginAttempts()
        {
            return _root.GetSection("ApplicationSettings:ValidateLoginAttempts").Value == "True";
        }

        public static string GetCookieSessionTimeout()
        {
            return _root.GetSection("ApplicationSettings:CookieSessionTimeout").Value;
        }

        public static string GetCookieDomain()
        {
            return _root.GetSection("ApplicationSettings:CookieDomain").Value;
        }

        public static bool GetIsJlrServer()
        {
            return _root.GetSection("ApplicationSettings:IsJLRServer").Value == "True"; 
        }

        public static double GetPasswordAttemptWindow()
        {
            double dPasswordAttemptWindow;
            if (double.TryParse(_root.GetSection("ApplicationSettings:passwordAttemptWindow").Value, out dPasswordAttemptWindow))
                return dPasswordAttemptWindow;
            return 0;
        }
        public static string GetEmailHost()
        {
            return _root.GetSection("ApplicationSettings:EmailHost").Value;
        }

        public static string GetEmailDomain()
        {
            return _root.GetSection("ApplicationSettings:EmailDomain").Value;
        }

        public static string GetEmailUserName()
        {
            return _root.GetSection("ApplicationSettings:EmailUserName").Value;
        }

        public static string GetEmailPassword()
        {
            return _root.GetSection("ApplicationSettings:EmailPassword").Value;
        }

        public static string GetEmailFromAddress()
        {
            return _root.GetSection("ApplicationSettings:EmailFromAddress").Value;
        }

        public static string GetIsLiveServer()
        {
            return _root.GetSection("ApplicationSettings:IsLiveServer").Value;
        }

        public static string GetSendLiveEmail()
        {
            return _root.GetSection("ApplicationSettings:SendLiveEmail").Value;
        }

        public static string GetTestEmailAccounts()
        {
            return _root.GetSection("ApplicationSettings:TestEmailAccounts").Value;
        }

        public static string GetChangePasswordLink()
        {
            return _root.GetSection("ApplicationSettings:ChangePasswordLink").Value;
        }
    }
}
