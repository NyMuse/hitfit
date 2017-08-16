using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using hitfit.app.Models;

namespace hitfit.app.Converters
{
    public static class FormFileConverter
    {
        public static async Task<UploadImage> ConvertToUploadImageAsync(IFormFile image)
        {
            byte[] imageBytes = new byte[image.Length];
            using (var memoryStream = new MemoryStream(imageBytes))
            {
                await image.CopyToAsync(memoryStream);
            }

            var uploadImage = new UploadImage
            {
                FileName = image.FileName,
                Image = imageBytes
            };

            return uploadImage;
        }
    }
}
