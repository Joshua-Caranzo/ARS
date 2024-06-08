using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Identity.API.Data
{
    internal class Clients
    {
        public static List<Client> GetClients()
        {
            return new List<Client>{

          new Client{ClientId = "ctlient",

                     AllowedGrantTypes = GrantTypes.Implicit,
                     RequireConsent = false, AllowAccessTokensViaBrowser = true,
                     RequirePkce = true,
                     // where to redirect to after login
                     RedirectUris =
                         {"http://localhost:5173/signin-callback",
                          "http://localhost:5173/assets/silent-callback.html"},

                     // where to redirect to after logout
                     PostLogoutRedirectUris =
                         {"http://localhost:5173/signout-callback"},

                     AllowedScopes =
                         new List<string>{

                             IdentityServerConstants.StandardScopes.OpenId,
                             IdentityServerConstants.StandardScopes.Profile,
                             "payrollapi",
                             "refresh_token"
                         }},


            };
        }
    }
}