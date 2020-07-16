using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace APIAsClient.Controllers
{
    [ApiController]
    public class GetTokenFromID4Controller : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GetTokenFromID4Controller(IHttpClientFactory factory)
        {
            this._httpClientFactory = factory;
        }

        [Route("api/CallAPI")]
        public async Task<IActionResult> CallAPI()
        {
            var client = _httpClientFactory.CreateClient();

            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = "https://localhost:44323/connect/token",
                ClientId = "client1_id",
                ClientSecret = "client1_secret",
                Scope = "api1.read"
            });

            if (tokenResponse.HttpResponse.IsSuccessStatusCode)
            {
                var apiClient = _httpClientFactory.CreateClient();

                apiClient.SetBearerToken(tokenResponse.AccessToken);

                var apiResponse = await apiClient.GetAsync("https://localhost:44319/api/commands");

                switch(apiResponse.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        var apiResult = await apiResponse.Content.ReadAsStringAsync();
                        return Ok(apiResult);
                    case System.Net.HttpStatusCode.Unauthorized:
                        return Unauthorized();
                    case System.Net.HttpStatusCode.NotFound:
                        return NotFound();
                    default:
                        return BadRequest();
                }

                return BadRequest();
            }
            else
                return BadRequest();
        }
    }
}
