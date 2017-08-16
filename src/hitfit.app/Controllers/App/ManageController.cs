using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using hitfit.app.Models;
using hitfit.app.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace hitfit.app.Controllers.App
{
    public class ManageController : Controller
    {
        public ManageController()
        {
        }

        [HttpGet]
        public IActionResult UploadImages()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadImages(List<IFormFile> images)
        {
            //if (images != null)
            //{
            //    foreach (var image in images)
            //    {
            //        if (image.Length > 0)
            //        {
            //            using (var ms = new MemoryStream())
            //            {
            //                await image.CopyToAsync(ms);
            //                var imageBytes = ms.ToArray();

            //                var imageObject = new ImageObject
            //                {
            //                    RelationType = "User",
            //                    OwnerId = 1,
            //                    Image = imageBytes
            //                };

            //                await _imageService.SaveImageAsync(imageObject);
            //            }
            //        }
            //    }

            //    return Ok();
            //}

            return View();
        }
    }
}