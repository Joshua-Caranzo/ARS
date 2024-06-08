using Identity.API.Entities;
using System.Threading.Tasks;

namespace Identity.API.Interfaces
{
    public interface IUserService
    {
        Task<LoginResult> Login(string userName, string password);
        Task<User> GetUserByUserNameAsync(string username);
    }
}
