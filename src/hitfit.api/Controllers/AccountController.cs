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
using hitfit.api.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace hitfit.api.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly HitFitDbContext _context;

        public IConfigurationRoot Configuration { get; }

        public AccountController(HitFitDbContext context)
        {
            _context = context;

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();
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

                var now = DateTime.UtcNow;
                // создаем JWT-токен
                var jwt = new JwtSecurityToken(
                    issuer: Configuration["JwtTokenConfiguration:ValidIssuer"],
                    audience: Configuration["JwtTokenConfiguration:ValidAudience"],
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromDays(1)),
                    signingCredentials:
                    new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["JwtTokenConfiguration:Key"])),
                        SecurityAlgorithms.HmacSha256));

                try
                {
                    var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                    var response = new
                    {
                        access_token = encodedJwt,
                        username = identity.Name
                    };

                    // сериализация ответа
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonConvert.SerializeObject(response,
                        new JsonSerializerSettings { Formatting = Formatting.Indented }));
                }
                catch (Exception e)
                {
                    Response.StatusCode = 400;
                    await Response.WriteAsync(e.Message);
                }

                
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
            user.PasswordSalt = Convert.ToBase64String(passwordSalt);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", "Users", new { id = user.Id }, user);
        }

        private async Task<ClaimsIdentity> GetIdentity(string username, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Username == username);

            if (user != null)
            {
                var passwordHash = this.HashPassword(password, Convert.FromBase64String(user.PasswordSalt));

                if (user.Password.Equals(passwordHash))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, user.Username)
                    };
                    ClaimsIdentity claimsIdentity =
                        new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                            ClaimsIdentity.DefaultRoleClaimType);
                    return claimsIdentity;
                }

                return null;
            }

            return null;
        }

        private string HashPassword(string password, byte[] salt)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }

        private byte[] GeneratePasswordSalt()
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return salt;
        }
    }
}