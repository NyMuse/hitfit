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
using Microsoft.Extensions.Logging;
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
        private readonly ILogger _logger;

        public AccountController(HitFitDbContext context, SignInManager<User> signInManager, UserManager<User> userManager, IConfiguration configuration, IUserClaimsPrincipalFactory<User> claimsPrincipalFactory, ILogger<AccountController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
            _claimsPrincipalFactory = claimsPrincipalFactory;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ErrorMessage"] = null;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, bool remember = false, string returnUrl = null)
        {
            if (this.Request.Form.ContainsKey("submit.Login"))
            {
                var user = await _userManager.FindByNameAsync(username) ?? await _userManager.FindByEmailAsync(username);

                if (user == null)
                {
                    _logger.LogInformation("Login failed. User [{0}] not exists.", username);
                    ViewData["ErrorMessage"] = "Пользователь с таким именем или e-mail не существует.";
                    return View();
                }

                var result = await _signInManager.PasswordSignInAsync(user, password, remember, false);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User [{0}] logged in.", username);
                    return RedirectToLocal(returnUrl);
                }
                
                if (result.IsLockedOut)
                {
                    _logger.LogInformation("Login failed. User [{0}] is locked out.", username);
                    ViewData["ErrorMessage"] = "Аккаунт заблокирован.";
                    return View();
                }
                else
                {
                    _logger.LogInformation("Login failed. Invalid password for user [{0}].", username);
                    ViewData["ErrorMessage"] = "Неверный пароль.";
                    return View();
                }
            }

            ViewData["ErrorMessage"] = null;
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            _logger.LogInformation("User [{0}] logged out.", this.User.Identity.Name);

            await _signInManager.SignOutAsync();
            
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Token()
        {
            var username = Request.Form["username"];
            var password = Request.Form["password"];

            var user = await _userManager.FindByNameAsync(username) ?? await _userManager.FindByEmailAsync(username);

            if (user == null)
            {
                return BadRequest("User not found.");
            }

            if (!await _userManager.CheckPasswordAsync(user, password))
            {
                return BadRequest("Invalid password.");
            }

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

        [HttpGet]
        public IActionResult Register()
        {
            ViewData["ErrorMessage"] = null;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string userName, string password, string passwordConfirm, string email, string phoneNumber, string userFirstName, string userMiddleName, string userLastName)
        {
            if (!password.Equals(passwordConfirm))
            {
                ViewData["ErrorMessage"] = "Введенные пароли не совпадают";
                return View();
            }

            if (this.Request.Form.ContainsKey("submit.Register"))
            {
                var user = new User
                {
                    UserName = userName,
                    Email = email,
                    PhoneNumber = userName,
                    FirstName = userFirstName,
                    LastName = userLastName
                };

                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    _logger.LogInformation("User [{0}] successfully registered.", userName);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    _logger.LogInformation("Registration failed. {0}", string.Join(" ", result.Errors.Select(e => e.Description)));
                    ViewData["ErrorMessage"] = string.Join(Environment.NewLine, result.Errors.Select(e => e.Description));
                    return View();
                }
            }

            ViewData["ErrorMessage"] = null;
            return View();
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}