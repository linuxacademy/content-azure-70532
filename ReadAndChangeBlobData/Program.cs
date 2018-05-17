using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadAndChangeBlobData
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
            try
            {
                await container.CreateIfNotExistsAsync();

                const string ImageToUpload = "bleu.jpg";
                var blockBlob = container.GetBlockBlobReference("bleu.jpg");
                
                // Create or overwrite the blockBlob with contents from a local file.
                using (var fileStream = System.IO.File.OpenRead(ImageToUpload)) {
                    blockBlob.UploadFromStream(fileStream);
                }

                foreach (var blob in container.ListBlobs()) {
                    Console.WriteLine(blob.Uri);
                }

                using (var fileStream = System.IO.File.OpenWrite("bleu2.jpg")) { 
                    await blockBlob.DownloadToStreamAsync(fileStream);
                }
            }
            catch (StorageException ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                throw;
            }
        }
    }
}
