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
            ViewData["ErrorMessage"] = null;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, bool remember = false)
        {
            if (this.Request.Form.ContainsKey("submit.Login"))
            {
                var user = await _userManager.FindByNameAsync(username) ?? await _userManager.FindByEmailAsync(username);

                if (user == null)
                {
                    ViewData["ErrorMessage"] = "������������ � ����� ������ ��� e-mail �� ����������.";
                    return View();
                }

                var result = await _signInManager.PasswordSignInAsync(user, password, remember, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                
                if (result.IsLockedOut)
                {
                    ViewData["ErrorMessage"] = "������� ������������.";
                    return View();
                }
                else
                {
                    ViewData["ErrorMessage"] = "�������� ������.";
                    return View();
                }
            }

            ViewData["ErrorMessage"] = null;
            return View();
        }

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
        public async Task<IActionResult> Register(string userName, string password, string email, string phoneNumber, string userFirstName, string userMiddleName, string userLastName)
        {
            if (this.Request.Form.ContainsKey("submit.Register"))
            {
                var user = new User
                {
                    UserName = userName,
                    Email = email,
                    PhoneNumber = userName,
                    FirstName = userFirstName,
                    //MiddleName = userMiddleName,
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
    }
}