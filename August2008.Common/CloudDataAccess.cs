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
            : this(ConfigurationManager.ConnectionStrings["August2008Azure"].ConnectionString)
        {              
        }
        public CloudDataAccess(string connectionString)
        {
            _storageAccount = CloudStorageAccount.Parse(connectionString);          
        }
        public void AddBlob(string container, string blobName, Stream stream)
        {
            var blobClient = _storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(container);
            container.CreateIfNotExists();
            container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            var blobBlock = container.GetBlockBlobReference(blobName);
            blobBlock.UploadFromStream(stream);
        }

    }
}
