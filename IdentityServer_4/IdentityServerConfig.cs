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
        public static IEnumerable<Client> GetClients()  => new List<Client> 
        { 
            new Client 
            { 
                ClientName = "Client1",
                ClientId = "client1_id",
                ClientSecrets = {new Secret("client1_secret".Sha256()) },
                AllowedScopes = new[] { "api1.read", "api1.write" },
                AllowedGrantTypes = GrantTypes.ClientCredentials
            }
        };

        public static IEnumerable<ApiResource> GetAPIResources() => new List<ApiResource>
        {
            new ApiResource
            {
                Name = "CommanderWebAPI",
                Scopes = new[] { "api1.read", "api1.write" },
                DisplayName = "Commander Web API"
            },
            new ApiResource
            {
                Name = "Api2",
                Scopes = new[] { "api2.read", "api2.write" },
                DisplayName = "API 2"
            }
        };

        public static IEnumerable<ApiScope> GetAPIScopes() => new List<ApiScope>
        {
            new ApiScope("api1.read"),
            new ApiScope("api1.write")
        };

        public static IEnumerable<IdentityResource> GetIdentityResources() => new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email()
        };
    }
}
