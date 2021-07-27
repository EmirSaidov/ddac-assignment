using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDAC_Assignment_Mining_Commerce.Services
{
    public class StorageAccountService
    {
        private AppSettingService _appSetting;

        public StorageAccountService(AppSettingService _appSetting) {
            this._appSetting = _appSetting;
        }

        //Connect to the correct storage account and retrieve the container
        public CloudStorageAccount connectStorageAccount()
        {
            return CloudStorageAccount.Parse(this._appSetting.getConnectionString("StorageAccountConnection"));
        }

        public CloudStorageAccount connectAnalyticsAccount()
        {
            return CloudStorageAccount.Parse(this._appSetting.getConnectionString("AnalyticsAccountConnection"));
        }

    }
}
