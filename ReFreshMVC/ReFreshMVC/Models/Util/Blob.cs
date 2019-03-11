using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ReFreshMVC.Models.Util
{
    public class Blob
    {
        public CloudStorageAccount CloudStorageAccount { get; set; }
        public CloudBlobClient CloudBlobClient { get; set; }

        public Blob(IConfiguration configuration)
        {
            CloudStorageAccount = CloudStorageAccount.Parse(configuration.GetConnectionString("BlobConnection"));
            CloudBlobClient = CloudStorageAccount.CreateCloudBlobClient();
        }

        /// <summary>
        /// retrieves an existing container by name, or 
        /// creates a new container by name if it doesn't already exist
        /// </summary>
        /// <param name="container"> name of container to retrieve/create </param>
        /// <returns> retrieved/created container </returns>
        public async Task<CloudBlobContainer> GetContainer(string container)
        {
            CloudBlobContainer cbc = CloudBlobClient.GetContainerReference(container.ToLower());
            await cbc.CreateIfNotExistsAsync();
            await cbc.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            return cbc;
        }

        /// <summary>
        /// gets specified blob reference in specified
        /// </summary>
        /// <param name="filename"> blob to find </param>
        /// <param name="containerName"> container where blob lives </param>
        /// <returns></returns>
        public async Task<CloudBlob> GetBlob(string filename, string containerName)
        {
            // can use anon data type and call direct if GetContainer weren't there
            // use anon type because not sure what Azure will return
            // var container = CloudBlobClient.GetContainerReference(containerName);
            CloudBlobContainer container = await GetContainer(containerName);

            CloudBlob blob = container.GetBlobReference(filename);
            return blob;
        }

        /// <summary>
        /// upload file to a container
        /// </summary>
        /// <param name="container"> container to target </param>
        /// <param name="filename"> name of file to upload </param>
        /// <param name="filepath"> path to file </param>
        public async void UploadFile(CloudBlobContainer container, string filename, string filepath)
        {
            var blobFile = container.GetBlockBlobReference(filename);
            await blobFile.UploadFromFileAsync(filepath);
        }

    }
}
