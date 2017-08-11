using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hitfit.app.Models.Dictionaries;
using Microsoft.AspNetCore.Http;
using hitfit.app.Models;

namespace hitfit.app.Services
{
    public interface IImageService
    {
        Task SaveImagesToDiskAsync(int userId, ImageRelationType relationType, int ownerId, List<IFormFile> images);

        Task<List<string>> GetImagesFromDiskAsync(int userId, ImageRelationType relationType, int ownerId);
    }
}
