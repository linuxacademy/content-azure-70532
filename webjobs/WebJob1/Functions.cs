using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;

namespace WebJob1
{
    public class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static void ProcessQueueMessage([QueueTrigger("jobs")] string message, TextWriter log)
        {
            Console.WriteLine("The message is: " + message);
            log.WriteLine(message);
        }

        public static void CopyBlob(
            [BlobTrigger("input/{name}")] TextReader input,
            [Blob("output/{name}")] out string output)
        {
            output = input.ReadToEnd();
        }

    }
}
