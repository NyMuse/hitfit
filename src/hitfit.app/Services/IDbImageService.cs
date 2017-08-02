using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hitfit.app.Models;

namespace hitfit.app.Services
{
    public interface IDbImageService
    {
        Task<int> SaveImageAsync(ImageObject image);

        Task<ImageObject> GetImageAsync(int id);

        Task<List<ImageObject>> GetImagesAsync(int ownerId, string relationType);
    }
}
