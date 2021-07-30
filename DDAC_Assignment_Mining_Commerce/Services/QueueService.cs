using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDAC_Assignment_Mining_Commerce.Services
{
    public class QueueService
    {
        private readonly StorageAccountService _storage;
        public QueueService(StorageAccountService _storage) {
            this._storage = _storage;
        }

        public CloudQueueClient getClientAgent()
        {
            CloudStorageAccount account = this._storage.connectStorageAccount();
            return account.CreateCloudQueueClient();
        }

        public CloudQueue getQueue(string queue_name)
        {
            CloudQueue queue = getClientAgent().GetQueueReference(queue_name);
            return queue.CreateIfNotExistsAsync().Result ? getQueue(queue_name) : queue;
        }
    }
}
