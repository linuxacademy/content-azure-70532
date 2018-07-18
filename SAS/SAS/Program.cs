using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAS
{
    class Program
    {
        static void Main(string[] args)
        {
            //var token = GetAccountSASToken();
            var token = GetAccountSASTokenViaPolicyName("ImagesSAP", "images");

            Console.WriteLine(token);
        }

        static string GetAccountSASToken()
        {
            // To create the account SAS, you need to use your shared key credentials. Modify for your account.
            const string ConnectionString = "";
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);

            // Create a new access policy for the account.
            SharedAccessAccountPolicy policy = new SharedAccessAccountPolicy()
            {
                Permissions = SharedAccessAccountPermissions.Read,
                Services = SharedAccessAccountServices.Blob,
                ResourceTypes = SharedAccessAccountResourceTypes.Object,
                SharedAccessExpiryTime = DateTime.UtcNow.AddHours(24),
                Protocols = SharedAccessProtocol.HttpsOnly
            };

            // Return the SAS token.
            return storageAccount.GetSharedAccessSignature(policy);
        }

        private static async Task CreateStoredAccessPolicyAsync(CloudBlobContainer container, string policyName)
        {
            // Create a new shared access policy and define its constraints.
            // The access policy provides create, write, read, list, and delete permissions.
            SharedAccessBlobPolicy sharedPolicy = new SharedAccessBlobPolicy()
            {
                // When the start time for the SAS is omitted, the start time is assumed to be the time when the storage service receives the request.
                // Omitting the start time for a SAS that is effective immediately helps to avoid clock skew.
                SharedAccessExpiryTime = DateTime.UtcNow.AddHours(24),
                Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.List |
                    SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.Create | SharedAccessBlobPermissions.Delete
            };

            // Get the container's existing permissions.
            BlobContainerPermissions permissions = await container.GetPermissionsAsync();

            // Add the new policy to the container's permissions, and set the container's permissions.
            permissions.SharedAccessPolicies.Add(policyName, sharedPolicy);
            await container.SetPermissionsAsync(permissions);
        }


        static string GetAccountSASTokenViaPolicyName(string policyName, string containerName)
        {
            // To create the account SAS, you need to use your shared key credentials. Modify for your account.
            const string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=la70532stg;AccountKey=/ZbchNmDCkvj6Wmsiumw8JKq1mT+EqdwbbTAZPbKloPMtNCwwKax9DA1s2JeaC2ZzuxktiUsOIHKa92rmmP5DQ==;EndpointSuffix=core.windows.net";
            var storageAccount = CloudStorageAccount.Parse(ConnectionString);

            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(containerName);
            var sas = container.GetSharedAccessSignature(null, policyName);
            return sas;
        }

        private static void CreateCORSPolicy(CloudBlobClient blobClient)
        {
            var serviceProperties = blobClient.GetServiceProperties();
            serviceProperties.Cors = new CorsProperties();
            serviceProperties.Cors.CorsRules.Add(new CorsRule()
            {
                AllowedHeaders = new List<string>() { "*" },
                AllowedMethods = CorsHttpMethods.Put | CorsHttpMethods.Get | CorsHttpMethods.Head | CorsHttpMethods.Post,
                AllowedOrigins = new List<string>() { "*" },
                ExposedHeaders = new List<string>() { "*" },
                MaxAgeInSeconds = 1800 // 30 minutes
            });
            blobClient.SetServiceProperties(serviceProperties);
        }
    }
}
