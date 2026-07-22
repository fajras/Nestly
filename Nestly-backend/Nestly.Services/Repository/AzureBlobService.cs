using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nestly.Services.Exceptions;

namespace Nestly.Services.Repository
{
    public class AzureBlobService
    {
        private readonly BlobContainerClient _container;
        private readonly ILogger<AzureBlobService> _logger;

        public AzureBlobService(IConfiguration config, ILogger<AzureBlobService> logger)
        {
            _logger = logger;

            var conn = config["AzureBlob:ConnectionString"];
            var containerName = config["AzureBlob:ContainerName"];

            var client = new BlobServiceClient(conn);
            _container = client.GetBlobContainerClient(containerName);
            _container.CreateIfNotExists();
        }

        public async Task<string> UploadBlogImageAsync(long blogId, Stream fileStream, string extension)
        {
            try
            {
                var blobName = $"blogPostImage{blogId}{extension}";
                var blob = _container.GetBlobClient(blobName);

                await blob.UploadAsync(fileStream, overwrite: true);
                return blob.Uri.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upload blog image for blog {BlogId}.", blogId);
                throw new BusinessException("Failed to upload image. Please try again.");
            }
        }

        public async Task DeleteBlogImageAsync(long blogId)
        {
            var prefixes = new[] { ".png", ".jpg", ".jpeg" };

            try
            {
                foreach (var ext in prefixes)
                {
                    var blob = _container.GetBlobClient($"blogPostImage{blogId}{ext}");
                    await blob.DeleteIfExistsAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete blog image for blog {BlogId}.", blogId);
                throw new BusinessException("Failed to delete image. Please try again.");
            }
        }
    }
}
