using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace Identity.API.Helper
{
    public sealed class EmailUtilities
    {
        private static string _emailHost;
        private static string _emailDomain;
        private static string _emailUserName;
        private static string _emailPassword;
        private static string _emailFromAddress;
        private static bool _sendLiveEmail;
        private static List<string> _testEmailAccounts;

        private static void SetVars()
        {
            _emailHost = StringUtilities.GetEmailHost();
            _emailDomain = StringUtilities.GetEmailDomain();
            _emailUserName = StringUtilities.GetEmailUserName();
            _emailPassword = StringUtilities.GetEmailPassword();
            _emailFromAddress = StringUtilities.GetEmailFromAddress();
            _sendLiveEmail = (StringUtilities.GetSendLiveEmail().ToLower() == "true");
            _testEmailAccounts = StringUtilities.GetTestEmailAccounts().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public static Exception SendEmail(string to, string message, string subject)
        {
             return SendEmail(to, null, message, subject);
        }

        public static Exception SendEmail(string to, string cc, string message, string subject)
        {
            SetVars();

            try
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailFromAddress),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true
                };

                if (!_sendLiveEmail)
                {
                    //Email to test email accounts
                    foreach (string email in _testEmailAccounts)
                    {
                        mailMessage.To.Add(new MailAddress(email));
                    }
                }
                else
                {
                    mailMessage.To.Add(new MailAddress(to));
                }
                if (cc != null)
                    mailMessage.CC.Add(new MailAddress(cc));

                var client = new SmtpClient();
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Timeout = 20000;
                client.UseDefaultCredentials = true;
                //client.Credentials = new NetworkCredential(_emailUserName, _emailPassword, _emailDomain);

                client.Host = _emailHost;
                client.Port = 25;

                client.Send(mailMessage);
            }
            catch (Exception ex)
            {
                return ex;
            }
            return null;
        }
    }
 
}
