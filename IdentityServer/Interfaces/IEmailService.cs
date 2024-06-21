using System.Threading;
using System.Threading.Tasks;

namespace Identity.API.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body, CancellationToken ct);
    }
}
