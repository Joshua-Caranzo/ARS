using Identity.API.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.API.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string body, CancellationToken ct)
        {
            try
            {
                var smtpSettings = _configuration.GetSection("SmtpSettings");

                var host = smtpSettings["Host"];
                var port = int.Parse(smtpSettings["Port"]);
                var username = smtpSettings["Username"];
                var password = smtpSettings["Password"];

                using (var client = new SmtpClient(host, port))
                {
                    client.Credentials = new NetworkCredential(username, password);
                    client.EnableSsl = true;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(username),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    };
                    mailMessage.To.Add(toEmail);

                    await client.SendMailAsync(mailMessage);
                }
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Format error while sending email: {ex.Message}");
                throw; 
            }
            catch (SmtpException ex)
            {
                Console.WriteLine($"SMTP error while sending email: {ex.StatusCode} - {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                // Handle other unexpected exceptions
                Console.WriteLine($"An error occurred while sending email: {ex.Message}");
                throw; // Re-throw to propagate the exception
            }
        }

    }
}
