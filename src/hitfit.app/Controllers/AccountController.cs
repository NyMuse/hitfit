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
using hitfit.app.ViewModels;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using User = hitfit.app.Models.User;

namespace hitfit.app.Controllers
{
    //[Route("/")]
    public class AccountController : Controller
    {
        private readonly HitFitDbContext _context;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IConfigurationRoot _configuration;

        public AccountController(HitFitDbContext context, SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            _configuration = builder.Build();
        }

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }
        
        public async Task<IActionResult> Login()
        {
            ViewData["Message"] = "Login page.";

            if (this.Request.Method == "POST")
            {
                if (this.Request.Form.ContainsKey("submit.Login"))
                {
                    var username = this.Request.Form["username"];
                    var password = this.Request.Form["password"];

                    var result = await _signInManager.PasswordSignInAsync(username, password, false, false);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    if (result.IsLockedOut)
                    {
                        //_logger.LogWarning(2, "User account locked out.");
                        //return View("Lockout");
                        return BadRequest("User account locked out.");
                    }
                    else
                    {
                        return BadRequest("Invalid login attempt.");
                        //ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        //return View(model);
                    }
                }

                
            }

            return View();
        }
        
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
        
        public async Task<IActionResult> Token()
        {
            var username = Request.Form["username"];
            var password = Request.Form["password"];

            var user = await _userManager.FindByNameAsync(username);
                //await _context.Users.SingleOrDefaultAsync(x => x.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));
            
            if (user != null)
            {
                if (await _userManager.CheckPasswordAsync(user, password))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
                        new Claim(ClaimTypes.NameIdentifier, user.UserName),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Role, user.IsAdministrator ? "admin" : "user"),
                        new Claim(ClaimTypes.GivenName, user.FirstName),
                        new Claim(ClaimTypes.Surname, user.LastName),
                        new Claim(ClaimTypes.Email, user.Email)
                    };

                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token",
                        ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

                    var now = DateTime.UtcNow;
                    var jwt = new JwtSecurityToken(
                        issuer: _configuration["JwtTokenConfiguration:ValidIssuer"],
                        audience: _configuration["JwtTokenConfiguration:ValidAudience"],
                        notBefore: now,
                        claims: claimsIdentity.Claims,
                        expires: now.Add(TimeSpan.FromDays(1)),
                        signingCredentials:
                        new SigningCredentials(
                            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JwtTokenConfiguration:Key"])),
                            SecurityAlgorithms.HmacSha256));

                    var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                    
                    var response = new
                    {
                        access_token = encodedJwt,
                        username = claimsIdentity.Name,
                        role = claimsIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value
                    };

                    Response.ContentType = "application/json";

                    return Ok(JsonConvert.SerializeObject(response,
                        new JsonSerializerSettings
                        {
                            Formatting = Formatting.Indented,
                            PreserveReferencesHandling = PreserveReferencesHandling.Objects
                        }));
                }
            }

            return BadRequest("Innvalid credentials");
        }
        
        public async Task<IActionResult> Register()
        {
            ViewData["Message"] = "Your registration page.";
            
            if (this.Request.Method == "POST")
            {
                if (this.Request.Form.ContainsKey("submit.Register"))
                {
                    var password = this.Request.Form["password"];

                    var user = new User
                    {
                        UserName = this.Request.Form["username"],
                        Email = this.Request.Form["email"],
                        PhoneNumber = this.Request.Form["phonenumber"],
                        FirstName = this.Request.Form["userfirstname"],
                        MiddleName = this.Request.Form["usermiddlename"],
                        LastName = this.Request.Form["userlastname"],
                        SecurityStamp = Guid.NewGuid().ToString(),
                        PasswordSalt = Guid.NewGuid().ToString()
                    };

                    //var passwordSalt = GetPasswordSalt();

                    //user.PasswordHash = this.GetPasswordHash(password, passwordSalt);
                    //user.PasswordSalt = Convert.ToBase64String(passwordSalt);

                    //_context.Users.Add(user);
                    //await _context.SaveChangesAsync();

                    var result = await _userManager.CreateAsync(user, password);

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Home");
                }
            }

            return View();
        }

        //public IActionResult UpdateInfo()
        //{
        //    var userIdentity = this.User;

        //    var id = 0;
            

        //    if (this.Request.Method == "POST")
        //    {
        //        if (this.Request.Form.ContainsKey("submit.UpdateMeasurements"))
        //        {
        //            var userMeasurements = new UserMeasurementsDto
        //            {
        //                UserId = id,
        //                Growth = short.Parse(this.Request.Form["growth"]),
        //                Weight = short.Parse(this.Request.Form["weight"])
        //            };

        //            var stringData = JsonConvert.SerializeObject(userMeasurements);

        //            var createdUserMeasurements = this.PostAction<UserMeasurementsDto>("api/users/measurements", stringData);

        //            return RedirectToAction("Index", "Home");
        //        }
        //    }
        //    else
        //    {
        //        var user = this.GetAction<User>("/api/users/", id.ToString());

        //        if (user != null)
        //        {
        //            ViewBag.UserGrowth = user.UserMeasurements.Last().Growth;
        //            ViewBag.UserWeight = user.UserMeasurements.Last().Weight;
        //        }
        //    }

        //    return View();
        //}

        //private bool CheckPassword(User user, string password)
        //{
        //    var passwordHash = this.GetPasswordHash(password, Convert.FromBase64String(user.PasswordSalt));

        //    return user.PasswordHash.Equals(passwordHash);
        //}

        //private string GetPasswordHash(string password, byte[] salt)
        //{
        //    return Convert.ToBase64String(KeyDerivation.Pbkdf2(
        //        password: password,
        //        salt: salt,
        //        prf: KeyDerivationPrf.HMACSHA1,
        //        iterationCount: 10000,
        //        numBytesRequested: 256 / 8));
        //}

        //private byte[] GetPasswordSalt()
        //{
        //    byte[] salt = new byte[128 / 8];
        //    using (var rng = RandomNumberGenerator.Create())
        //    {
        //        rng.GetBytes(salt);
        //    }

        //    return salt;
        //}
    }
}