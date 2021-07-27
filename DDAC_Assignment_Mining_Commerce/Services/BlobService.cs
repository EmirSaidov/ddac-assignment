using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

		public CloudBlobContainer getContainerInfo(string container_name) {
			CloudBlobContainer container = getClientAgent().GetContainerReference(container_name);
			return container.CreateIfNotExistsAsync().Result ? getContainerInfo(container_name) : container;
		}

		// Generate blob name
		private string generateBlobName(IFormFile image)
		{
			return DateTime.Now.ToString("yyyyMMddHHmmssffff") + Path.GetExtension(image.FileName).ToLower();
		}

		// Get blob name
		private string getBlobName(string uri)
        {
			var processedString = uri
				.Replace("https://miningassignment.blob.core.windows.net/", "")
				.Replace("product/", "");
			//Debug.WriteLine(processedString);
			return processedString;
		}

		// Add blob item
		public string uploadToProductContainer(IFormFile image)
        {
			var blobItem = getContainerInfo("product").GetBlockBlobReference(generateBlobName(image));
			blobItem.UploadFromStreamAsync(image.OpenReadStream()).Wait();
			return blobItem.Uri.ToString();
        }

        // Delete blob item
        public bool deleteFromProductContainer(string uri)
        {
			if (uri == null)
            {
				return false;
            }
			var blobItem = getContainerInfo("product").GetBlockBlobReference(getBlobName(uri));
			return blobItem.DeleteIfExistsAsync().Result;
        }

    }
}
