using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TokenServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private string _key = "this_is_My_very_dumb_Secret_Key_hence_please_do_not_use_this";
        private string _issuer = "mywebsite.com";
        private string _audience = "clientaudience";

        [HttpPost]
        public IActionResult GetToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var signingCredential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken
                (
                    issuer: _issuer,
                    audience: _audience,
                    claims: new[]
                    {
                        new Claim("Claim1", "Value1"),
                        new Claim("Claim2", "Value2")
                    },
                    notBefore: DateTime.Now,
                    expires: DateTime.Now.AddDays(10),
                    signingCredentials: signingCredential
                );

            var stringToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return Ok(new { access_token = stringToken });
        }
    }
}
