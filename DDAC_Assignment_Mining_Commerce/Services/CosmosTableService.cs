using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDAC_Assignment_Mining_Commerce.Services
{
    public class CosmosTableService
    {
        private readonly AppSettingService _appSetting;
        public CosmosTableService(
            AppSettingService _appSetting
            ){
            this._appSetting = _appSetting;
        }

        public CloudStorageAccount connectCosmosAccount()
        {
            return CloudStorageAccount.Parse(this._appSetting.getConnectionString("CosmosTableConnection"));
        }

        public CloudTableClient getClientAgent()
        {
            CloudStorageAccount account = this.connectCosmosAccount();
            return account.CreateCloudTableClient(new TableClientConfiguration());
        }

        public CloudTable getTable(string table_name)
        {
            CloudTable table = getClientAgent().GetTableReference(table_name);
            return table.CreateIfNotExistsAsync().Result ? getTable(table_name) : table;
        }

    }
}
