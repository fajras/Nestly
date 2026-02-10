using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;

namespace Nestly.Services.Repository
{
    public class AzureBlobService
    {
        private readonly BlobContainerClient _container;

        public AzureBlobService(IConfiguration config)
        {
            var conn = config["AzureBlob:ConnectionString"];
            var containerName = config["AzureBlob:ContainerName"];

            var client = new BlobServiceClient(conn);
            _container = client.GetBlobContainerClient(containerName);
            _container.CreateIfNotExists();
        }

        public async Task<string> UploadBlogImageAsync(long blogId, Stream fileStream, string extension)
        {
            var blobName = $"blogPostImage{blogId}{extension}";
            var blob = _container.GetBlobClient(blobName);

            await blob.UploadAsync(fileStream, overwrite: true);
            return blob.Uri.ToString();
        }

        public async Task DeleteBlogImageAsync(long blogId)
        {
            var prefixes = new[] { ".png", ".jpg", ".jpeg" };

            foreach (var ext in prefixes)
            {
                var blob = _container.GetBlobClient($"blogPostImage{blogId}{ext}");
                await blob.DeleteIfExistsAsync();
            }
        }
    }
}
