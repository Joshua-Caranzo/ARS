using Identity.API.Interfaces;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Identity.API.Services
{
    public class UserProfileService : IProfileService
    {
        private readonly IUserService _userService;
        public UserProfileService(IUserService userService)
        {
            _userService = userService;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var name = context.Subject.GetSubjectId();
            var user = await _userService.GetUserByUserNameAsync(name);
            if (user != null)
            {
                context.IssuedClaims.Add(new Claim("uid", user.Id.ToString()));
                context.IssuedClaims.Add(new Claim("name", user.UserName));
                context.IssuedClaims.Add(new Claim(JwtClaimTypes.Role, user.Id.ToString()));
                context.IssuedClaims.Add(new Claim("userTypeId", user.UserTypeId.ToString()));
                // Send an email with this link

            }

        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var name = context.Subject.GetSubjectId();
            var user = await _userService.GetUserByUserNameAsync(name);
            if (user == null)
            {
                context.IsActive = false;
            }
            else
            {
                context.IsActive = user.Active;
            }
        }
    }
}
