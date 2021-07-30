using Azure;
using DDAC_Assignment_Mining_Commerce.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DDAC_Assignment_Mining_Commerce.Services
{
    public class TableService
    {
        private StorageAccountService _storage;

        public TableService(StorageAccountService _storage)
        {
            this._storage = _storage;
        }

        public CloudTableClient getClientAgent() {
            CloudStorageAccount account = this._storage.connectStorageAccount();
            return account.CreateCloudTableClient();
        }

        public CloudTable getTable(string table_name)
        {
            CloudTable table = getClientAgent().GetTableReference(table_name);
            return table.CreateIfNotExistsAsync().Result ? getTable(table_name) : table;
        }

        public async void Subscribe(Subscription subscription)
        {
            Subscription exist = await GetSubscription(subscription.RowKey, subscription.PartitionKey);
            if (exist == null)
            {
                TableOperation tableOperation = TableOperation.Insert(subscription);
                _ = getTable("subscriptions").ExecuteAsync(tableOperation);
            } else
            {
                subscription.ETag = "*";
                TableOperation tableOperation = TableOperation.Delete(subscription);
                _ = getTable("subscriptions").ExecuteAsync(tableOperation);
            }
        }

        public async Task<Subscription> GetSubscription(string rowKey, string partitionKey)
        {
            TableOperation tableOperation = TableOperation.Retrieve<Subscription>(partitionKey, rowKey);
            TableResult tableResult = await getTable("subscriptions").ExecuteAsync(tableOperation);
            return tableResult.Result as Subscription;
        }

        public async Task<List<Subscription>> GetSubscriptionsByPK(string partitionKey)
        {
            List<Subscription> subscriptions = new List<Subscription>();
            TableQuery<Subscription> partitionScanQuery = new TableQuery<Subscription>().Where
               (TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));

            TableContinuationToken token = null;
            // Page through the results
            do
            {
                TableQuerySegment<Subscription> segment = await getTable("subscriptions").ExecuteQuerySegmentedAsync(partitionScanQuery, token);
                token = segment.ContinuationToken;
                foreach (Subscription subscription in segment)
                {
                    subscriptions.Add(subscription);
                }
            }
            while (token != null);

            return subscriptions;
        }
    }
}
