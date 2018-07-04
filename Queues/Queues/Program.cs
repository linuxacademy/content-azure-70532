using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queues
{
    class Program
    {
        static void Main(string[] args)
        {
            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnection"));
            var queueClient = storageAccount.CreateCloudQueueClient();
            var queue = queueClient.GetQueueReference("tasks");
            queue.CreateIfNotExists();

            //var queueMessage = new CloudQueueMessage("Hello from the application");
            // var ttl = TimeSpan.FromSeconds(30);
            //queue.AddMessage(queueMessage);

            Task.Factory.StartNew(() =>
            {
                var i = 0;
                while (true)
                {
                    var queueMessage = new CloudQueueMessage($"The value is: {i++}");
                    queue.AddMessage(queueMessage);
                    Task.Delay(100).Wait();
                }
            });
            CloudBlobContainer cont;
            cont.getblob
            while (true)
            {
                var messages = queue.GetMessages(10);
                Console.WriteLine($"Received {messages.Count()} message");
                foreach (var message in messages) { 
                    Console.WriteLine(message.AsString);
                    //Task.Delay(1000).Wait();
                    queue.DeleteMessage(message);
                }
                Task.Delay(500).Wait();
            }

            Console.WriteLine("Press any key to continue");
            Console.ReadLine();
        }
    }
}
