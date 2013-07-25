using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using August2008.Common.Interfaces;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace August2008.Common
{
    public class CloudDataAccess
    {
        private CloudStorageAccount _storageAccount;

        public CloudDataAccess()
            : this(ConfigurationManager.ConnectionStrings["August2008Cloud"].ConnectionString)
        {              
        }
        public CloudDataAccess(string connectionString)
        {
            _storageAccount = CloudStorageAccount.Parse(connectionString);   
        }
        public void AddBlob(string container, string blobName, Stream stream, string contentType = null)
        {
            var blobClient = _storageAccount.CreateCloudBlobClient();
            var containerRef = blobClient.GetContainerReference(container);
            containerRef.CreateIfNotExists();
            containerRef.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            var blobBlock = containerRef.GetBlockBlobReference(blobName);
            if (!string.IsNullOrWhiteSpace(contentType))
            {
                blobBlock.Properties.ContentType = contentType;
            }
            blobBlock.UploadFromStream(stream);
        }
        public List<Uri> ListBlobsUri(string directoryName, string subDirectoryName)
        {            
            var directory = GetBlobDirectory(directoryName, subDirectoryName);
            var blobsUri = new List<Uri>();
            foreach (var item in directory.ListBlobs(true, BlobListingDetails.Metadata))
            {
                blobsUri.Add(item.Uri);
            }
            return blobsUri;
        }
        public Uri GetBaseUri(string directoryName, string subDirectoryName) 
        {
            var directory = GetBlobDirectory(directoryName, subDirectoryName);
            return directory.Uri;
        }
        public bool DeleteBlob(string directoryName, string subDirectoryName, string blobName) 
        {
            var directory = GetBlobDirectory(directoryName, subDirectoryName);
            var blobBlock = directory.GetBlockBlobReference(blobName);
            return blobBlock.DeleteIfExists();
        }
        private CloudBlobDirectory GetBlobDirectory(string directoryName, string subDirectoryName)
        {
            var blobClient = _storageAccount.CreateCloudBlobClient();
            var directory = blobClient.GetContainerReference(directoryName);
            var subDirectory = directory.GetDirectoryReference(subDirectoryName);
            return subDirectory;
        }
    }
}
