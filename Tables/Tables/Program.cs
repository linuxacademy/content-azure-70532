using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tables
{
    class Program
    {
        static void Main(string[] args)
        {
            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnection"));
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference("Customers");
            table.CreateIfNotExists();

            var customer = new CustomerEntity("Harp", "Walter")
            {
                Email = "Walter@contoso.com",
                PhoneNumber = "425-555-0101"
            };

            InsertCustomerAsync(table, customer).Wait();
            GetCustomerAsync(table, "Harp", "Walter").Wait();
            InsertCustomerAsync(table, new CustomerEntity("Heydt", "Michael")).Wait();
            InsertCustomerAsync(table, new CustomerEntity("Heydt", "Bleu")).Wait();
            QueryCustomerAsync(table, "Heydt").Wait();
            var michael = GetCustomerAsync(table, "Heydt", "Michael").Result;
            michael.Email = "michael@localhost";
            UpdateCustomerAsync(table, michael).Wait();
            //DeleteCustomerAsync(table, "Heydt", "Michael").Wait();


            InsertCustomersAsync(table, new[] {
                new CustomerEntity("Doe", "John"),
                new CustomerEntity("Doe", "Jane")
                }).Wait();

            QueryCustomerAsync(table, "Doe").Wait();
        }

        private static async Task DeleteCustomerAsync(CloudTable table, string lastName, string firstName)
        {
            var customer = GetCustomerAsync(table, lastName, firstName).Result;
            var delete = TableOperation.Delete(customer);
            await table.ExecuteAsync(delete);
            Console.WriteLine("Deleted: " + customer);
        }

        private static async Task UpdateCustomerAsync(CloudTable table, CustomerEntity customer)
        {
            var update = TableOperation.Replace(customer);
            await table.ExecuteAsync(update);
        }

        private static async Task QueryCustomerAsync(CloudTable table, string lastName)
        {
            var query = new TableQuery<CustomerEntity>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, lastName));
            table.ExecuteQuery(query).ToList().ForEach(customer =>
            {
                Console.WriteLine(customer);
            });
            await Task.CompletedTask;
        }

        static async Task<CustomerEntity> GetCustomerAsync(CloudTable table, string partitionKey, string rowKey)
        {
            var retrieve = TableOperation.Retrieve<CustomerEntity>(partitionKey, rowKey);
            var result = await table.ExecuteAsync(retrieve);
            //Console.WriteLine(result.Result);
            return (CustomerEntity)result.Result;
        }

        static async Task InsertCustomerAsync(CloudTable table, CustomerEntity customer)
        {
            var insert = TableOperation.Insert(customer);
            await table.ExecuteAsync(insert);
        }

        static async Task InsertCustomersAsync(CloudTable table, CustomerEntity[] customers)
        {
            var batch = new TableBatchOperation();

            customers.ToList().ForEach(batch.Insert);

            await table.ExecuteBatchAsync(batch);
        }
    }
}
