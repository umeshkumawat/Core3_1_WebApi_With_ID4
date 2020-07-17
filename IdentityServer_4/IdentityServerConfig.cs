using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer_4
{
    public static class IdentityServerConfig
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() => new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(),
            new IdentityResource()
            { 
                Name = "rc.scope",
                UserClaims = new[] { "rc.grandma" }
            }
        };
        public static IEnumerable<Client> GetClients()  => new List<Client> 
        { 
            new Client 
            { 
                ClientName = "Client1 Client Credentials Flow",
                ClientId = "client1_id",
                ClientSecrets = {new Secret("client1_secret".Sha256()) },
                AllowedScopes = new[] { "api1.read", "api1.write" },
                AllowedGrantTypes = GrantTypes.ClientCredentials
            },
            new Client
            {
                ClientName = "Client2 Authorization Code Flow",
                ClientId = "client2_id",
                ClientSecrets = {new Secret("client2_secret".Sha256()) },
                AllowedScopes = new[] 
                { 
                    "api1.read", 
                    "api1.write", 
                    "openid", 
                    "profile", 
                    "rc.scope" 
                },
                RedirectUris = new[] {"https://localhost:44366/signin-oidc" },
                PostLogoutRedirectUris = new[] {"https://localhost:44366/Home/Index" },

                RequireConsent = false, // Will not prompt for user's consent (like facebook/gmail does)

                AllowedGrantTypes = GrantTypes.Code
            }
        };


        // अगर हम access_token में कुछ add करना चाहते है तो उसे ApiResource में अदद करना होगा (जैसे User Claims include करना)
        public static IEnumerable<ApiResource> GetAPIResources() => new List<ApiResource>
        {
            new ApiResource
            {
                Name = "CommanderWebAPI",
                Scopes = new[] { "api1.read", "api1.write" },
                DisplayName = "Commander Web API"
            }
        };

        public static IEnumerable<ApiScope> GetAPIScopes() => new List<ApiScope>
        {
            new ApiScope("api1.read"),
            new ApiScope("api1.write")
        };

    }
}
