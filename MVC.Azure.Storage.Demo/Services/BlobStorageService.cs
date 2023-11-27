using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;

namespace MVC.Azure.Storage.Demo.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly IConfiguration _configuration;
        private string containerName = "attendeeimages";

        public BlobStorageService(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        private async Task<BlobContainerClient> GetContainerClient()
        {
            try
            {
                BlobContainerClient container = new BlobContainerClient(_configuration["StorageConnectionString"], containerName);
                await container.CreateIfNotExistsAsync();

                return container;
            }
            catch (Exception ex)
            {
                throw;

            }
        }

        public async Task<string> UploadBlob(IFormFile formFile, string imageName,
            string? originalBlobName = null)
        {
            var blobName = $"{imageName}{Path.GetExtension(formFile.FileName)}";
            var container = await GetContainerClient();

            if (!string.IsNullOrEmpty(originalBlobName))
            {
                await RemoveBlob(originalBlobName);
            }

            using var memoryStream = new MemoryStream();
            formFile.CopyTo(memoryStream);
            memoryStream.Position = 0;
            var blob = container.GetBlobClient(blobName);
            await blob.UploadAsync(memoryStream, overwrite: true);
            return blobName;
        }

        public async Task<string> GetBlobUrl(string imageName)
        {
            var container = await GetContainerClient();
            var blob = container.GetBlobClient(imageName);

            BlobSasBuilder builder = new BlobSasBuilder()
            {
                BlobContainerName = blob.BlobContainerName,
                BlobName = blob.Name,
                ExpiresOn = DateTime.UtcNow.AddMinutes(2),
                Protocol = SasProtocol.Https,
                Resource = "b"
            };
            builder.SetPermissions(BlobAccountSasPermissions.Read);

            return blob.GenerateSasUri(builder).ToString();
        }

        public async Task RemoveBlob(string imageName)
        {
            var container = await GetContainerClient();
            var blob = container.GetBlobClient(imageName);
            await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
        }

    }
}
