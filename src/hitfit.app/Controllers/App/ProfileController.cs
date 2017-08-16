using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hitfit.app.Converters;
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
        private readonly ProfileService _profileService;

        public ProfileController(IImageService imageService, UserManager<User> userManager, ProfileService profileService)
        {
            _imageService = imageService;
            _userManager = userManager;
            _profileService = profileService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(this.User.Identity.Name);
            var profile = await _profileService.GetFrofileAsync(user.Id);
            
            //var images =  await _imageService.GetImagesFromDiskAsync(user.Id, ImageRelationType.UserProgram, 1);

            ViewBag.Profile = profile;

            return View();
        }

        public async Task UpdatePersonalData(IFormFile file, string firstNameInput, string lastNameInput, string phoneNumberInput, DateTime birthdayInput, string notesInput)
        {
            var user = await _userManager.FindByNameAsync(this.User.Identity.Name);

            byte[] photo = null;
            if (file != null && file.Name != "select_image.png")
            {
                using (var ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);
                    photo = ms.ToArray();
                }
            }
            
            await _profileService.UpdatePersonalDataAsync(user.Id, photo, firstNameInput, lastNameInput, phoneNumberInput, birthdayInput, notesInput);

            //await _imageService.SaveImagesToDiskAsync(user.Id, ImageRelationType.UserProgram, 1, images);

            //return RedirectToAction("Index");
        }

        public async Task AddMeasurements(List<IFormFile> images)
        {
            var user = await _userManager.FindByNameAsync(this.User.Identity.Name);

            //var userProgram = _

            List<UploadImage> uploadImages = new List<UploadImage>();
            foreach (var image in images)
            {
                var uploadImage = await FormFileConverter.ConvertToUploadImageAsync(image);

                uploadImages.Add(uploadImage);
            }
        }
    }
}