
using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using SharedLibs.Interfaces;

namespace SharedLibs
{
    public class FilePersistenceService : IFileUploader, IFileDownloader
    {
        //TODO add Azure blob storage connection string here
        private const string ConnectionString = "";

        private const string BlobContainer = "shared-blob";

        private readonly BlobContainerClient _blobContainerClient;

        public FilePersistenceService(BlobContainerClient blobContainerClientClient = null)
        {
            _blobContainerClient = blobContainerClientClient ?? new BlobContainerClient(ConnectionString, BlobContainer);
        }

        public async Task UploadFileAsync(Guid fileId, Stream file)
        {
            var blobClient = _blobContainerClient.GetBlobClient(fileId.ToString());
            await blobClient.UploadAsync(file);
        }

        public async Task DownloadFile(Guid fileId, Stream file)
        {
            var blobClient = _blobContainerClient.GetBlobClient(fileId.ToString());
            await blobClient.DownloadToAsync(file);
        }
    }
}
