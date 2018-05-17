using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_1_Create_Storage_Account_CS
{
    class Program
    {
        static void Main(string[] args)
        {
            var credentials = new AzureCredentials(
                new UserLoginInformation {
                    ClientId = "Azure client Id",
                    UserName = "username",
                    Password = "Password" }, 
                "tenant Id", AzureEnvironment.AzureGlobalCloud);  

            var azure = Azure
                        .Configure()
                        .Authenticate(credentials)
                        .WithDefaultSubscription();
        }
    }
}
