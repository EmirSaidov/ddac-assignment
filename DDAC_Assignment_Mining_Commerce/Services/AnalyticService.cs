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

        public async Task pushAnalytics<T>(T analytics) where T:TableEntity,Analytics{
            CloudTable table = getTable(analytics.tableName());
            await table.ExecuteBatchAsync(analytics.operations());
        }
    }

    public interface Analytics{
        //Name of table for analytics
        public string tableName();
        public TableBatchOperation operations();
    }

    public class LoginAnalytics:TableEntity,Analytics {
        public LoginAnalytics() { }
        public LoginAnalytics(int user_id, DateTime login_time) {
            this.user_id = user_id;
            this.login_time= login_time;
            this.PartitionKey = user_id.ToString();
            this.RowKey = user_id.ToString();
        }
        public int user_id { get; set; }
        public DateTime login_time { get; set; }
        TableBatchOperation batch = new TableBatchOperation();
        public string tableName() {
            return "Login";
        }

        public TableBatchOperation operations() {
            batch.Insert(new LoginAnalytics(this.user_id, this.login_time));
            return batch;
        }
    }
}
