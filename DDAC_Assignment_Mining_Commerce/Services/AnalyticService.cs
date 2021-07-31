using Azure.Messaging.ServiceBus;
using DDAC_Assignment_Mining_Commerce.Models.Analytics;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDAC_Assignment_Mining_Commerce.Services
{
    public class AnalyticService
    {
        StorageAccountService _storage;
        private readonly AppSettingService _appSetting;
        private readonly string queueName = "analytic-log";
        public ServiceBusClient SBclient;
        private ServiceBusSender SBsender;
        public AnalyticService(StorageAccountService _storage, AppSettingService _appSetting) {
            this._storage = _storage;
            this._appSetting = _appSetting;
            this.SBclient = getSBClient();
            this.SBsender = getSBSender(SBclient,queueName);

        }

        public CloudTableClient getTableClientAgent()
        {
            CloudStorageAccount account = this._storage.connectAnalyticsAccount();
            return account.CreateCloudTableClient();
        }

        public CloudQueueClient getQueueClientAgent()
        {
            CloudStorageAccount account = this._storage.connectAnalyticsAccount();
            return account.CreateCloudQueueClient();
        }

        public CloudTable getTable(string table_name)
        {
            CloudTable table = getTableClientAgent().GetTableReference(table_name);
            return table.CreateIfNotExistsAsync().Result ? getTable(table_name) : table;
        }

        public CloudQueue getQueue(string queue_name)
        {
            CloudQueue queue = getQueueClientAgent().GetQueueReference(queue_name);
            return queue.CreateIfNotExistsAsync().Result ? getQueue(queue_name) : queue;
        }

        public ServiceBusClient getSBClient() { 
            return new ServiceBusClient(this._appSetting.getConnectionString("ServiceBusConnection"));
        }

        public ServiceBusSender getSBSender(ServiceBusClient client,string queueName) {
            return client.CreateSender(queueName);
        }

        public async Task logAnalytic<T>(T analytics) where T : TableEntity, AnalyticModel {
            var analyticJson = JsonConvert.SerializeObject(analytics);
            ServiceBusMessage msg = new ServiceBusMessage(analyticJson);
            msg.ApplicationProperties.Add("table", analytics.tableName());
            await this.SBsender.SendMessageAsync(msg);
        }

        public async Task enqueueErrorMsg(string msg) {
            CloudQueue queue = getQueue("analyticerror");
            await queue.AddMessageAsync(new CloudQueueMessage(msg));
        }

        public async Task demoErrorMsg(string msg) {
            ServiceBusMessage ErrorMsg = new ServiceBusMessage(msg);
            await this.SBsender.SendMessageAsync(ErrorMsg);
            
        }
    }

   

}
