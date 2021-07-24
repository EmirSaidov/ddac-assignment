using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace DDAC_Assignment_Mining_Commerce.Services
{
	public class BlobService
	{
		private StorageAccountService _storage;

		public BlobService(StorageAccountService _storage) {
			this._storage = _storage;
		}

		//Create agent
		public CloudBlobClient getClientAgent() {
			CloudStorageAccount account = this._storage.connectStorageAccount();
			return account.CreateCloudBlobClient();
		}

		public CloudBlobContainer getContainerInfo(string container_name, bool create_new = false) {
			CloudBlobContainer container = getClientAgent().GetContainerReference(container_name);
			if (create_new) {
				//CreateIfNotExists Async Returns true if container does not previously exist
				return container.CreateIfNotExistsAsync().Result ? getContainerInfo(container_name) : container;
			}
			return container;
		}

		public bool createContainer(string container_name) {
			return getContainerInfo(container_name).CreateIfNotExistsAsync().Result;
		}

	}
}
