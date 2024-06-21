using Identity.API.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.API.Interfaces
{
    public interface IUserService
    {
        Task<LoginResult> Login(string userName, string password);
        Task<User> GetUserByUserNameAsync(string username);
        Task<User> FindByEmailAsync(string email);
        Task<IdentityResult> ResetPassword(User user, string password, CancellationToken ct);
    }
}
