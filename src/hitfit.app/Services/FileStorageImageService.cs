using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hitfit.app.Models.Dictionaries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using hitfit.app.Models;
using System;

namespace hitfit.app.Services
{
    public class FileStorageImageService : IImageService
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public FileStorageImageService(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task SaveImagesToDiskAsync(int userId, ImageRelationType relationType, int ownerId, List<UploadImage> images)
        {
            string contentRootPath = _hostingEnvironment.ContentRootPath;
            var userPath = Path.Combine(contentRootPath, "uploads", userId.ToString());
            var path = Path.Combine(userPath, relationType.ToString(), ownerId.ToString());

            Directory.CreateDirectory(path);

            foreach (var image in images)
            {
                if (image.Image.Length > 0)
                {
                    var extension = Path.GetExtension(image.FileName);
                    var fileName = Guid.NewGuid().ToString() + extension;

                    var filePath = Path.Combine(path, fileName);

                    using (var sourceStream = File.Open(filePath, FileMode.OpenOrCreate))
                    {
                        sourceStream.Seek(0, SeekOrigin.End);
                        await sourceStream.WriteAsync(image.Image, 0, image.Image.Length);
                    }

                    //using (var fileStream = new FileStream(filePath, FileMode.Create))
                    //{
                    //    await image.CopyToAsync(fileStream);
                    //}
                }
            }
        }

        public async Task<List<string>> GetImagesFromDiskAsync(int userId, ImageRelationType relationType, int ownerId)
        {
            string contentRootPath = _hostingEnvironment.ContentRootPath;
            var userPath = Path.Combine(contentRootPath, "uploads", userId.ToString());
            var path = Path.Combine(userPath, relationType.ToString(), ownerId.ToString());

            if (Directory.Exists(path))
            {
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

            return null;
        }
    }
}
