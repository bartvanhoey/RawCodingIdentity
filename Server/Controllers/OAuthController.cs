using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Server.Controllers
{
    public class OAuthController : Controller
    {

        [HttpGet]
        public IActionResult Authorize(
            string response_type,
            string client_id,
            string redirect_uri,
            string scope,
            string state)
        {
            var query = new QueryBuilder
            {
                { "redirectUri", redirect_uri },
                { "state", state }
            };

            return View(model: query.ToString());
        }

        [HttpPost]
        public IActionResult Authorize(
            string username,
            string redirectUri,
            string state)
        {
            const string code = "BBaaaaaaa";

            var query = new QueryBuilder
            {
                { "code", code },
                { "state", state }
            };

            return Redirect($"{redirectUri}{query}");
        }

 

        [HttpGet]
        public async Task<IActionResult> Token(string grant_type, string code, string redirect_uri, string client_id)
        {

            // some mechanism to validating the code


            // CREATE ACCESSTOKEN
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub,"some_id"),
                new Claim("Granny", "cookie")
            };

            var secretsBytes = Encoding.UTF8.GetBytes(Constants.Secret);
            var key = new SymmetricSecurityKey(secretsBytes);
            var algorithm = SecurityAlgorithms.HmacSha256;

            var signingCredentials = new SigningCredentials(key, algorithm);

            var token = new JwtSecurityToken(
                Constants.Issuer,
                Constants.Audience,
                claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signingCredentials
              );

            var access_token = new JwtSecurityTokenHandler().WriteToken(token);

            var response = new { 
                access_token, 
                token_type = "Bearer",
                raw_claim = "oauthTutorial"
            };

            var jsonResponse = JsonConvert.SerializeObject(response);
            var bytesResponse = Encoding.UTF8.GetBytes(jsonResponse);
            await Response.Body.WriteAsync(bytesResponse, 0, bytesResponse.Length);

            return Redirect(redirect_uri);
        }
    }
}
