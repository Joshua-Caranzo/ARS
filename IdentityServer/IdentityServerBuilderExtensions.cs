using Identity.API.Interfaces;
using Identity.API.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.API
{
    public static class IdentityServerBuilderExtensions
    {
        public static IIdentityServerBuilder AddUserStore(this IIdentityServerBuilder builder)
        {
            builder.Services.AddTransient<IUserService, UserService>();
            builder.AddProfileService<UserProfileService>();
            return builder;
        }
    }
}
