using Azure.Storage.Blobs;
using healthspanmd.core.Services.FileSystem;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.filesystem
{
    public class FileSystemManager : IFileSystemManager
    {
        private readonly IConfiguration _config;

        public FileSystemManager(
            IConfiguration config
            )
        {
            _config = config;
        }

        public void SaveFile(string containerName, string fileName, byte[] fileData)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(_config.GetConnectionString("AzureStorageConnection"));

            var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            if (!blobContainerClient.Exists())
                blobContainerClient.Create();


            BlobClient blobClient = blobContainerClient.GetBlobClient(fileName);

            BinaryData binaryData = new BinaryData(fileData);
            blobClient.Upload(binaryData, true);
        }

        public byte[] GetFile(string containerName, string fileName)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(_config.GetConnectionString("AzureStorageConnection"));
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(fileName);
            using (var ms = new MemoryStream())
            {
                blobClient.DownloadTo(ms);
                return ms.ToArray();
            }
        }

    }
}
