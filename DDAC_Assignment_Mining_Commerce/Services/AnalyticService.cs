using DDAC_Assignment_Mining_Commerce.Models.Analytics;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDAC_Assignment_Mining_Commerce.Services
{
    public class AnalyticService
    {
        StorageAccountService _storage;
        public AnalyticService(StorageAccountService _storage) {
            this._storage = _storage;
        }

        public CloudTableClient getClientAgent()
        {
            CloudStorageAccount account = this._storage.connectAnalyticsAccount();
            return account.CreateCloudTableClient();
        }

        public CloudTable getTable(string table_name)
        {
            CloudTable table = getClientAgent().GetTableReference(table_name);
            return table.CreateIfNotExistsAsync().Result ? getTable(table_name) : table;
        }

        public async Task pushAnalytics<T>(T analytics) where T:TableEntity,AnalyticModel{
            CloudTable table = getTable(analytics.tableName());
            await table.ExecuteBatchAsync(analytics.operations());
        }
    }

   

}
