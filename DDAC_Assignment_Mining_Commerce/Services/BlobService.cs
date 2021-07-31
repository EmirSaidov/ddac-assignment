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

		private async Task<Boolean> checkIfBlobExists(string container_name, string blob_name)
        {
			try
			{
				return await getClientAgent().GetContainerReference(container_name).GetBlockBlobReference(blob_name).ExistsAsync();
			}
			catch (Exception ex) {
				Console.WriteLine("Error with verifying blob existence " + ex);
				return false;
			}
		}

		// Get blob name
		#nullable enable
		public string getBlobURLFromStorage(string container_name, string blob_name, string default_url) {
			string blobString = $"https://miningassignment.blob.core.windows.net/{container_name}/{blob_name}";
			return checkIfBlobExists(container_name, blob_name).Result ? blobString : default_url;
		}
		private string getBlobName(string uri)
        {
			var processedString = uri
				.Replace("https://miningassignment.blob.core.windows.net/", "")
				.Replace("product/", "");
			//Debug.WriteLine(processedString);
			return processedString;
		}

		// Add blob item
		#nullable enable
		public string uploadImgToBlobContainer(string container_name, string blob_name, IFormFile image,string? contentType = null) {
			var blobItem = getContainerInfo(container_name).GetBlockBlobReference(blob_name);
			//Uploading image using post will set content type as octet-stream
			if (contentType != null) {
				blobItem.Properties.ContentType = contentType;
			}
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
