using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using hitfit.app.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace hitfit.app.Services
{
    public class BlobStorageImageService : IImageService
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly CloudBlobContainer _container;
        private readonly ILogger _logger;

        public BlobStorageImageService(IHostingEnvironment hostingEnvironment, ILogger<BlobStorageImageService> logger)
        {
            _logger = logger;

            _hostingEnvironment = hostingEnvironment;

            var storageAccount = new CloudStorageAccount(
                new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(
                    "hitfitstorage",
                    "dRA3QtkQ5lXBnMQAK5PRp+fli2T3Kr3qDxkIJo0Mgt7ys5uiQx0aSepf6y9tzjjabvZ/P7O+BGePMTB1p2qnjw=="), false);

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            _container = blobClient.GetContainerReference("client-images");

            _container.CreateIfNotExistsAsync();

            _logger.LogInformation("BlobStorageImageService initiated.");
        }

        public async Task SaveImagesToDiskAsync(int userId, ImageRelationType relationType, int ownerId, List<UploadImage> images)
        {
            try
            {
                foreach (var image in images)
                {
                    if (image.Image.Length > 0)
                    {
                        var extension = Path.GetExtension(image.FileName);
                        var fileName = Guid.NewGuid().ToString() + extension;

                        var path = Path.Combine(new[]
                        {
                            userId.ToString(), relationType.ToString(), ownerId.ToString(), fileName
                        });

                        CloudBlockBlob blockBlob = _container.GetBlockBlobReference(path);
                        await blockBlob.UploadFromByteArrayAsync(image.Image, 0, image.Image.Length);
                    }
                }

                _logger.LogInformation("{0} images saved to blob storage. Images count: {1}", relationType.ToString(), images.Count);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
            }
        }

        public async Task<List<string>> GetImagesFromDiskAsync(int userId, ImageRelationType relationType, int ownerId)
        {
            var images = new List<string>();

            var path = string.Format("{0}/{1}/{2}/", userId, relationType.ToString(), ownerId);

            BlobContinuationToken token = null;
            do
            {
                var directory = _container.GetDirectoryReference(path);

                BlobResultSegment resultSegment = await directory.ListBlobsSegmentedAsync(token);
                token = resultSegment.ContinuationToken;

                foreach (IListBlobItem item in resultSegment.Results)
                {
                    if (item.GetType() == typeof(CloudBlockBlob))
                    {
                        string image;

                        CloudBlockBlob blob = (CloudBlockBlob)item;

                        using (MemoryStream ms = new MemoryStream())
                        {
                            await blob.DownloadToStreamAsync(ms);
                            image = Convert.ToBase64String(ms.ToArray());
                        }

                        images.Add(image);
                    }
                }
            } while (token != null);

            return images;
        }
    }
}
