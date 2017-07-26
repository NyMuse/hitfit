using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using hitfit.app.Models;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace hitfit.app.ApiControllers
{
    public class ApiController : Controller
    {
        protected readonly HitFitDbContext Context;

        public IConfigurationRoot Configuration { get; }

        public ApiController(HitFitDbContext context)
        {
            this.Context = context;

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();
        }

        protected string GenerateToken(ClaimsIdentity identity)
        {
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: Configuration["JwtTokenConfiguration:ValidIssuer"],
                audience: Configuration["JwtTokenConfiguration:ValidAudience"],
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromDays(1)),
                signingCredentials:
                new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["JwtTokenConfiguration:Key"])),
                    SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);

        }

        protected async Task<ClaimsIdentity> GetIdentity(string username, string password)
        {
            var user = await Context.Users.SingleOrDefaultAsync(x => x.Login == username);

            if (user != null)
            {
                var passwordHash = this.HashPassword(password, Convert.FromBase64String(user.PasswordSalt));

                if (user.Password.Equals(passwordHash))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Role, user.IsAdministrator ? "admin" : "user")
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

        protected string HashPassword(string password, byte[] salt)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }

        protected byte[] GeneratePasswordSalt()
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
