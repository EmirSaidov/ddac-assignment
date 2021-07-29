using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDAC_Assignment_Mining_Commerce.Services
{
    public class CosmosSQLService
    {
        private readonly AppSettingService _appSetting;
        private CosmosIdentity miningCommerceAccount;
        private CosmosClient miningCommerceClient;

        public CosmosSQLService(AppSettingService _appSetting) {
            this._appSetting = _appSetting;
            this.miningCommerceAccount = new CosmosIdentity(_appSetting.getCosmosProperty("URI"), _appSetting.getCosmosProperty("primary_key"));
            miningCommerceClient = getClientAgent(this.miningCommerceAccount);
        }


        private CosmosClient getClientAgent(CosmosIdentity cosmos_acc) {
            return new CosmosClient(cosmos_acc.uri, cosmos_acc.endpoint);
        }

        //can only handle 1 account for now
        private async Task<Database> CreateDatabaseAsync(string database_name)
        {
            // Create a new database
            return await miningCommerceClient.CreateDatabaseIfNotExistsAsync(database_name);

        }

        // <CreateContainerAsync>
        //private async Task<Container> CreateContainerAsync()
        //{
            // Create a new container
            //return await .CreateContainerIfNotExistsAsync("UserType", "/UserType");
            
        //}
    }

    public class CosmosIdentity{

        public CosmosIdentity() { }
        public CosmosIdentity(string uri, string endpoint) {
            this.uri = uri;
            this.endpoint = endpoint;
        }
        public string uri;
        public string endpoint;
    }
}
