using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobAndContainerMetadata
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().run().Wait();
        }

        public async Task run()
        {
            //var endpoint = "";
            // var storageAccount = CloudStorageAccount.Parse(endpoint);
            var storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnection"));
            var blobClient = storageAccount.CreateCloudBlobClient();

            var container = blobClient.GetContainerReference("democontainerblockblob");
            dumpMetadata(container);

            //container.Metadata.Add("Owner", "Mike");
            //container.SetMetadata();

            try
            {
                var blockBlob = container.GetBlockBlobReference("bleu.jpg");
                dumpMetadata(blockBlob);
            }
            catch (StorageException ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                throw;
            }
        }

        private void dumpMetadata(CloudBlockBlob blockBlob)
        {
            foreach (var kvp in blockBlob.Metadata)
            {
                Console.WriteLine(kvp);
            }
        }

        private void dumpMetadata(CloudBlobContainer container)
        {
            container.FetchAttributes();
            foreach (var key in container.Metadata)
            {
                Console.WriteLine(key);
            }
        }
    }

}
