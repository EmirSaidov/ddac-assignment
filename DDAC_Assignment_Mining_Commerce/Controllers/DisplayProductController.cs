using DDAC_Assignment_Mining_Commerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DDAC_Assignment_Mining_Commerce.Services;
using DDAC_Assignment_Mining_Commerce.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos;
using System.Net;
using System.Configuration;

namespace DDAC_Assignment_Mining_Commerce.Controllers
{
    public class DisplayProductController : Controller
    {
        private readonly MiningCommerceContext _context;
        private readonly BlobService _blobService;
        public DisplayProductController(MiningCommerceContext context, BlobService blobService)
        {
            _context = context;
            _blobService = blobService;

        }
        // The Azure Cosmos DB endpoint for running this sample.
        private static readonly string EndpointUri = ConfigurationManager.AppSettings["EndPointUri"];

        // The primary key for the Azure Cosmos account.
        private static readonly string PrimaryKey = ConfigurationManager.AppSettings["PrimaryKey"];


        // The Cosmos client instance
        private CosmosClient cosmosClient;

        // The database we will create
        private Database database;

        // The container we will create.
        private Container container;

        // The name of the database and container we will create
        private string databaseId = "Cart"; 


        public async Task<IActionResult> AddToCart(int? id)
        {
            string containerId = HttpContext.Session.Get<BuyerModel>("AuthRole").user.email.ToString();
            this.cosmosClient = new CosmosClient(EndpointUri, PrimaryKey, new CosmosClientOptions() { ApplicationName = "CosmosDBDotnetQuickstart" });
            var product = await _context.Product.FindAsync(id);
            this.database = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
            this.container = await this.database.CreateContainerIfNotExistsAsync(containerId, "/partitionKey");

            try
            {
                int? throughput = await this.container.ReadThroughputAsync();
                if (throughput.HasValue)
                {
                    int newThroughput = throughput.Value + 100;
                    // Update throughput
                    await this.container.ReplaceThroughputAsync(newThroughput);
                }
            }
            catch (CosmosException cosmosException) when (cosmosException.StatusCode == HttpStatusCode.BadRequest)
            {
            }

            Models.CosmosCartModel cart = new Models.CosmosCartModel
            {
                Id = product.ID.ToString() + HttpContext.Session.Get<BuyerModel>("AuthRole").ID.ToString(),
                PartitionKey = HttpContext.Session.Get<BuyerModel>("AuthRole").user.fullname.ToString(),
                BuyerID = HttpContext.Session.Get<BuyerModel>("AuthRole").ID.ToString(),
                ProductID = product.ID.ToString(),
                ProductQuantity = 1,
                IsRegistered = false
            };

            try
            {
                // Read the item to see if it exists.  
                ItemResponse<Models.CosmosCartModel> cartResponse = await this.container.ReadItemAsync<Models.CosmosCartModel>(cart.Id, new PartitionKey(cart.PartitionKey));

            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                // Create an item in the container representing the Andersen family. Note we provide the value of the partition key for this item, which is "Andersen"
                ItemResponse<Models.CosmosCartModel> cartResponse = await this.container.CreateItemAsync<Models.CosmosCartModel>(cart, new PartitionKey(cart.PartitionKey));

            }

            return View("../DisplayProducts/Display", await _context.Product.ToListAsync());
            // Create a family object for the Wakefield family

        }

        public async Task<IActionResult> Cart()
        {
            return View("../DisplayProducts/Cart", await _context.Product.Where(product => RetreiveCart().Result.Contains(product.ID.ToString())).ToListAsync());


        }

        public async Task<List<string>> RetreiveCart()
        {
            string containerId = HttpContext.Session.Get<BuyerModel>("AuthRole").user.email.ToString();
            this.cosmosClient = new CosmosClient(EndpointUri, PrimaryKey, new CosmosClientOptions() { ApplicationName = "CosmosDBDotnetQuickstart" });
            this.database = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
            this.container = await this.database.CreateContainerIfNotExistsAsync(containerId, "/partitionKey");
            var sqlQueryText = "SELECT c.ProductID FROM c WHERE c.BuyerID = '" + HttpContext.Session.Get<BuyerModel>("AuthRole").ID + "'";

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<Models.CosmosCartModel> queryResultSetIterator = this.container.GetItemQueryIterator<Models.CosmosCartModel>(queryDefinition);

            List<string> items = new List<string>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Models.CosmosCartModel> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (Models.CosmosCartModel cart in currentResultSet)
                {
                    items.Add(cart.ProductID);
                }
            }
            return items;


        }


        public async Task<IActionResult> DeleteOneItem(int? id)
        {
            string containerId = HttpContext.Session.Get<BuyerModel>("AuthRole").user.email.ToString();
            this.cosmosClient = new CosmosClient(EndpointUri, PrimaryKey, new CosmosClientOptions() { ApplicationName = "CosmosDBDotnetQuickstart" });
            this.database = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
            this.container = await this.database.CreateContainerIfNotExistsAsync(containerId, "/partitionKey");
            var product = await _context.Product.FindAsync(id);

            var partitionKeyValue = HttpContext.Session.Get<BuyerModel>("AuthRole").user.fullname.ToString();
            var cartId = product.ID.ToString() + HttpContext.Session.Get<BuyerModel>("AuthRole").ID.ToString();

            // Delete an item. Note we must provide the partition key value and id of the item to delete
            ItemResponse<Models.CosmosCartModel> cartResponse = await this.container.DeleteItemAsync<Models.CosmosCartModel>(cartId, new PartitionKey(partitionKeyValue));
            return await Cart();
        }








        public async Task<IActionResult> Checkout()
        {
            return View("../DisplayProducts/Pay", await _context.Product.Where(product => RetreiveCart().Result.Contains(product.ID.ToString())).ToListAsync());
        }
        public async Task<IActionResult> Receipt()
        {
            List<string> receipt = RetreiveCart().Result;

            string containerId = HttpContext.Session.Get<BuyerModel>("AuthRole").user.email.ToString();
            this.cosmosClient = new CosmosClient(EndpointUri, PrimaryKey, new CosmosClientOptions() { ApplicationName = "CosmosDBDotnetQuickstart" });
            this.database = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
            this.container = await this.database.CreateContainerIfNotExistsAsync(containerId, "/partitionKey");

            ContainerResponse containerResponse = await this.container.DeleteContainerAsync();

            return View("../DisplayProducts/Receipt", await _context.Product.Where(product => receipt.Contains(product.ID.ToString())).ToListAsync());
        }
       
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
