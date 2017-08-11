using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hitfit.app.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using hitfit.app.Models;
using Microsoft.AspNetCore.Http;

namespace hitfit.app.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IImageService _imageService;
        private readonly UserManager<User> _userManager;

        public ProfileController(IImageService imageService, UserManager<User> userManager)
        {
            _imageService = imageService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(this.User.Identity.Name);

            var images =  await _imageService.GetImagesFromDiskAsync(user.Id, ImageRelationType.UserProgram, 1);

            ViewBag.Images = images;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(List<IFormFile> images)
        {
            var user = await _userManager.FindByNameAsync(this.User.Identity.Name);

            await _imageService.SaveImagesToDiskAsync(user.Id, ImageRelationType.UserProgram, 1, images);

            return RedirectToAction("Index");
        }
    }
}