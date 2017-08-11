using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using hitfit.app.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace hitfit.app.Services
{
    public class BlobStorageImageService : IImageService
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public BlobStorageImageService(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task SaveImagesToDiskAsync(int userId, ImageRelationType relationType, int ownerId, List<IFormFile> images)
        {
            string contentRootPath = _hostingEnvironment.ContentRootPath;
            var userPath = Path.Combine(contentRootPath, "uploads", userId.ToString());
            var path = Path.Combine(userPath, relationType.ToString(), ownerId.ToString());

            Directory.CreateDirectory(path);

            foreach (var image in images)
            {
                if (image.Length > 0)
                {
                    var extension = Path.GetExtension(image.FileName);
                    var fileName = Guid.NewGuid().ToString() + extension;

                    var filePath = Path.Combine(path, fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }
                }
            }
        }

        public async Task<List<string>> GetImagesFromDiskAsync(int userId, ImageRelationType relationType, int ownerId)
        {
            string contentRootPath = _hostingEnvironment.ContentRootPath;
            var userPath = Path.Combine(contentRootPath, "uploads", userId.ToString());
            var path = Path.Combine(userPath, relationType.ToString(), ownerId.ToString());

            var fileNames = Directory.GetFiles(path, "*.jpg");

            var images = new List<string>();

            foreach (var fileName in fileNames)
            {
                var filePath = Path.Combine(path, fileName);
                FileInfo fileInfo = new FileInfo(filePath);
                byte[] image = new byte[fileInfo.Length];

                using (FileStream fs = fileInfo.OpenRead())
                {
                    await fs.ReadAsync(image, 0, image.Length);
                }

                images.Add(Convert.ToBase64String(image));
            }

            return images;
        }
    }
}
