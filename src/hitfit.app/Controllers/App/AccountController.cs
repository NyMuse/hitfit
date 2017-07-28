using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using hitfit.app.Models;
using hitfit.app.ViewModels;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using User = hitfit.app.Models.User;

namespace hitfit.app.Controllers.App
{
    public class AccountController : AppController
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly HitFitDbContext _context;
        //private readonly UserClaimsPrincipalFactory<User> _claimsPrincipalFactory;

        public AccountController(HitFitDbContext context, SignInManager<User> signInManager, UserManager<User> userManager)//, UserClaimsPrincipalFactory<User> claimsPrincipalFactory)
        {
            //_claimsPrincipalFactory = claimsPrincipalFactory;
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
            
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

                    //this.GetJwtToken(username, password);
                    var user = await _context.Users.SingleOrDefaultAsync(x => x.Login.Equals(username));

                    if (user != null)
                    {
                        var passwordHash = this.HashPassword(password, Convert.FromBase64String(user.PasswordSalt));

                        if (user.Password.Equals(passwordHash))
                        {

                            await _signInManager.SignInAsync(user, true);
                        }
                    }
                }

                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Register()
        {
            ViewData["Message"] = "Your registration page.";
            
            if (this.Request.Method == "POST")
            {
                if (this.Request.Form.ContainsKey("submit.Register"))
                {
                    var user = new User
                    {
                        Login = this.Request.Form["username"],
                        UserName = this.Request.Form["username"],
                        Password = this.Request.Form["password"],
                        Email = this.Request.Form["email"],
                        FirstName = this.Request.Form["userfirstname"],
                        MiddleName = this.Request.Form["usermiddlename"],
                        LastName = this.Request.Form["userlastname"],
                        SecurityStamp = Guid.NewGuid().ToString()
                    };

                    var passwordSalt = GeneratePasswordSalt();

                    var hashedPassword = this.HashPassword(user.Password, passwordSalt);

                    user.Password = hashedPassword;
                    user.PasswordSalt = Convert.ToBase64String(passwordSalt);

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    //var result = await _userManager.CreateAsync(user, user.Password);

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Home");
                    //return RedirectToAction("UpdateInfo", new { id = createdUser.Id });
                }
            }

            return View();
        }

        public IActionResult UpdateInfo()
        {
            var userIdentity = this.User;

            var id = 0;
            

            if (this.Request.Method == "POST")
            {
                if (this.Request.Form.ContainsKey("submit.UpdateMeasurements"))
                {
                    var userMeasurements = new UserMeasurementsDto
                    {
                        UserId = id,
                        Growth = short.Parse(this.Request.Form["growth"]),
                        Weight = short.Parse(this.Request.Form["weight"])
                    };

                    var stringData = JsonConvert.SerializeObject(userMeasurements);

                    var createdUserMeasurements = this.PostAction<UserMeasurementsDto>("api/users/measurements", stringData);

                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                var user = this.GetAction<User>("/api/users/", id.ToString());

                if (user != null)
                {
                    ViewBag.UserGrowth = user.UserMeasurements.Last().Growth;
                    ViewBag.UserWeight = user.UserMeasurements.Last().Weight;
                }
            }

            return View();
        }

        protected async Task<ClaimsIdentity> GetIdentity(string username, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Login == username);

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