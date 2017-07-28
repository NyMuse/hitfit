using System;
using System.Collections.Generic;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net;
using System.Net.Http;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using hitfit.app.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace hitfit.app.Controllers.Api
{
    [Produces("application/json")]
    [Route("[controller]")]
    public class AccountApiController : ApiController
    {
        public AccountApiController(HitFitDbContext context) : base(context)
        {
            
        }

        [HttpPost("token")]
        public async Task Token()
        {
            var username = Request.Form["username"];
            var password = Request.Form["password"];

            var identity = await GetIdentity(username, password);
            if (identity == null)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Invalid username or password.");
            }
            else
            {
                var encodedJwt = this.GenerateToken(identity);
                var response = new
                {
                    access_token = encodedJwt,
                    username = identity.Name,
                    role = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value
                };

                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonConvert.SerializeObject(response,
                    new JsonSerializerSettings { Formatting = Formatting.Indented, PreserveReferencesHandling = PreserveReferencesHandling.Objects }));
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var passwordSalt = GeneratePasswordSalt();

            var hashedPassword = this.HashPassword(user.Password, passwordSalt);

            user.Password = hashedPassword;
            //user.PasswordSalt = Convert.ToBase64String(passwordSalt);

            this.Context.Users.Add(user);
            await this.Context.SaveChangesAsync();

            return CreatedAtAction("GetUser", "Users", new { id = user.Id }, user);
        }
    }
}