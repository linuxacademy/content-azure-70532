using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blobs
{
    class Program
    {
        static void Main(string[] args)
        {
        var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnection"));
        var blobClient = storageAccount.CreateCloudBlobClient();
        var container = blobClient.GetContainerReference("images");
        container.CreateIfNotExists(BlobContainerPublicAccessType.Blob);

            //SetMetadata(container);
            DumpMetadata(container);

            //var blockBlob = container.GetBlockBlobReference("braque1.jpg");
            //using (var stream = System.IO.File.OpenRead(@"c:\braque1.jpg"))
            //{
            //    blockBlob.UploadFromStream(stream);
            //}

            //var blobs = container.ListBlobs();
            //blobs.ToList().ForEach(blob => Console.WriteLine(blob.Uri));

            //using (var stream = System.IO.File.OpenWrite(@"c:\braque_downloaded.jpg"))
            //{
            //    blockBlob.DownloadToStream(stream);
            //}

            //var copiedBlockBlob = container.GetBlockBlobReference("copied.jpg");
            //copiedBlockBlob.BeginStartCopy(blockBlob,
            //    new AsyncCallback(x => Console.WriteLine("Copy completed")),
            //    null);

            //var blockBlobInFolder = container.GetBlockBlobReference("jpg-images/braque3.jpg");
            //blockBlobInFolder.Properties.ContentType = "image/jpg";
            //using (var stream = System.IO.File.OpenRead(@"c:\braque2.jpg"))
            //{
            //    blockBlobInFolder.UploadFromStream(stream);
            //}


            Console.WriteLine("Press any key");
            Console.ReadKey();
        }

        public static void SetMetadata(CloudBlobContainer container)
        {
            container.Metadata.Clear();
            container.Metadata.Add("CreatedBy", "Michael");
            container.Metadata["LastUpdated"] = DateTime.Now.ToString();
            container.SetMetadata();
        }

        public static void DumpMetadata(CloudBlobContainer container)
        {
            container.FetchAttributes();
            Console.WriteLine($"Container {container.Uri} metadata:");
            container.Metadata.ToList().ForEach(kvp => Console.WriteLine($"{kvp.Key}: {kvp.Value}"));
        }

        public static void SetMetadata(CloudBlob blob)
        {
            blob.Metadata.Clear();
            blob.Metadata.Add("Owner", "Mike");
            blob.Metadata["LastUpdated"] = DateTime.Now.ToString();
            blob.SetMetadata();
        }

        public static void DumpMetadata(CloudBlob blob)
        {
            blob.FetchAttributes();
            Console.WriteLine($"Blob {blob.Uri} metadata:");
            blob.Metadata.ToList().ForEach(kvp => Console.WriteLine($"{kvp.Key}: {kvp.Value}"));
        }

    }
}
