using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Core.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrainWise.User;

namespace TrainWise.Services
{
    public static class StorageService
    {
        static CloudBlobContainer GetContainer(string container)
        {
            var account = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=trainwisevideos;AccountKey=86uSidC28jQiw+/FXNGgjfNdk3kxJLlPUViaVhSw5VY3IgxYMSsv7Fjl9BA+lLcuY6DVJRYT/b+lSipFcyLO2A==;EndpointSuffix=core.windows.net");
            var client = account.CreateCloudBlobClient();
            return client.GetContainerReference(container);
        }

        public static async Task<IList<string>> GetFilesListAsync(string videoContainer)
        {
            var container = GetContainer(videoContainer);

            var allBlobsList = new List<string>();
            BlobContinuationToken token = null;

            do
            {
                var result = await container.ListBlobsSegmentedAsync(token);
                if (result.Results.Count() > 0)
                {
                    var blobs = result.Results.Cast<CloudBlockBlob>().Select(b => b.Name);
                    allBlobsList.AddRange(blobs);
                }
                token = result.ContinuationToken;
            } while (token != null);

            return allBlobsList;
        }

        public static async Task<byte[]> GetFileAsync(string videoContainer, string name)
        {
            var container = GetContainer(videoContainer);

            var blob = container.GetBlobReference(name);
            if (await blob.ExistsAsync())
            {
                await blob.FetchAttributesAsync();
                byte[] blobBytes = new byte[blob.Properties.Length];

                await blob.DownloadToByteArrayAsync(blobBytes, 0);
                return blobBytes;
            }
            return null;
        }

        public static async Task<string> UploadFileAsync(string videoContainer, Stream stream, string videoName, Progress<StorageProgress> prog)
        {
            var container = GetContainer(videoContainer);
            await container.CreateIfNotExistsAsync();
            var fileBlob = container.GetBlockBlobReference(videoName);
            await fileBlob.UploadFromStreamAsync(stream, stream.Length, null, null, null, prog, new System.Threading.CancellationToken());

            return videoName;
        }

        public static async Task<bool> DeleteFileAsync(string videoContainer, string name)
        {
            var container = GetContainer(videoContainer);
            var blob = container.GetBlobReference(name);
            return await blob.DeleteIfExistsAsync();
        }

        public static async Task<bool> DeleteContainerAsync(string videoContainer)
        {
            var container = GetContainer(videoContainer);
            return await container.DeleteIfExistsAsync();
        }

        public static async Task AddVideo()
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=trainwisevideos;AccountKey=86uSidC28jQiw+/FXNGgjfNdk3kxJLlPUViaVhSw5VY3IgxYMSsv7Fjl9BA+lLcuY6DVJRYT/b+lSipFcyLO2A==;EndpointSuffix=core.windows.net");

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference("videocontainer");

            // Create the container if it doesn't already exist.
            await container.CreateIfNotExistsAsync();

            // Retrieve reference to a blob named "myblob".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference("videoblob");

            // Create the "myblob" blob with the text "Hello, world!"
            await blockBlob.UploadTextAsync("Hello, world!");
        }

    }
}
