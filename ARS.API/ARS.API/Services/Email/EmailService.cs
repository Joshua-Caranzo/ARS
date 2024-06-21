using System.Net;
using System.Net.Mail;

namespace ARS.API.Services.Email
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

                    await client.SendMailAsync(mailMessage, ct);
                }
            }
            catch (FormatException ex)
            {
                // Handle format-related exceptions (e.g., invalid email address)
                Console.WriteLine($"Format error while sending email: {ex.Message}");
                throw; // Re-throw to propagate the exception
            }
            catch (SmtpException ex)
            {
                // Handle SMTP-related exceptions (e.g., SMTP server issues)
                Console.WriteLine($"SMTP error while sending email: {ex.StatusCode} - {ex.Message}");
                throw; // Re-throw to propagate the exception
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
