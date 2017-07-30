using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using hitfit.app.Models;
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
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IUserClaimsPrincipalFactory<User> _claimsPrincipalFactory;

        public AccountController(HitFitDbContext context, SignInManager<User> signInManager, UserManager<User> userManager, IConfiguration configuration, IUserClaimsPrincipalFactory<User> claimsPrincipalFactory)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
            _claimsPrincipalFactory = claimsPrincipalFactory;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (this.Request.Form.ContainsKey("submit.Login"))
            {
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

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Token()
        {
            var username = Request.Form["username"];
            var password = Request.Form["password"];

            var user = await _userManager.FindByNameAsync(username);
            
            if (user != null)
            {
                if (await _userManager.CheckPasswordAsync(user, password))
                {
                    var claimsPrincipal = await _claimsPrincipalFactory.CreateAsync(user);

                    var now = DateTime.UtcNow;
                    var jwt = new JwtSecurityToken(
                        issuer: _configuration["JwtTokenConfiguration:ValidIssuer"],
                        audience: _configuration["JwtTokenConfiguration:ValidAudience"],
                        notBefore: now,
                        claims: claimsPrincipal.Claims,
                        expires: now.Add(TimeSpan.FromDays(1)),
                        signingCredentials:
                        new SigningCredentials(
                            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JwtTokenConfiguration:Key"])),
                            SecurityAlgorithms.HmacSha256));

                    var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                    
                    var response = new
                    {
                        access_token = encodedJwt,
                        username = claimsPrincipal.Identity.Name,
                        role = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value
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

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string userName, string password, string email, string phoneNumber, string userFirstName, string userMiddleName, string userLastName)
        {
            if (this.Request.Form.ContainsKey("submit.Register"))
            {
                var user = new User
                {
                    UserName = userName,
                    Email = email,
                    PhoneNumber = phoneNumber,
                    FirstName = userFirstName,
                    MiddleName = userMiddleName,
                    LastName = userLastName
                };

                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
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