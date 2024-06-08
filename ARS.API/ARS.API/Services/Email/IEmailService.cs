namespace ARS.API.Services.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body, CancellationToken ct);
    }
}
