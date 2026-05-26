using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;

namespace CrochetItAPI.Services
{
    public class BlobService
    {
        private readonly IConfiguration configuration;
        public BlobService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string GenerateUploadSas(string fileName)
        {
            var connectionString = configuration["AzureBlob:ConnectionString"];
            var containerName = configuration["AzureBlob:ContainerName"];
            var blobServiceClient = new BlobServiceClient(connectionString);
            var containerClient =
            blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(fileName);
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = containerName,
                BlobName = fileName,
                Resource = "b",
                ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(10)
            };
            sasBuilder.SetPermissions(BlobSasPermissions.Write);
            var storageSharedKeyCredential =
            new StorageSharedKeyCredential(
            blobServiceClient.AccountName,
            GetAccountKeyFromConnectionString(connectionString));
            var sasToken = sasBuilder.ToSasQueryParameters(
            storageSharedKeyCredential);
            return blobClient.Uri + "?" + sasToken;
        }
        private string GetAccountKeyFromConnectionString(string connectionString)
        {
            var parts = connectionString.Split(';');
            foreach (var part in parts)
            {
                if (part.StartsWith("AccountKey="))
                {
                    return part.Replace("AccountKey=", "");
                }
            }
            return string.Empty;
        }
    }
}
