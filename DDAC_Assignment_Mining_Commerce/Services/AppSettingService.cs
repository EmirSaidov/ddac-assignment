using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace DDAC_Assignment_Mining_Commerce.Services
{
    public class AppSettingService
    {
        public readonly IConfiguration _config;
        public AppSettingService(IConfiguration configuration) {
            this._config = configuration;
        }

        public string getConnectionString(string settingKey) {
            return this._config.GetValue<string>("ConnectionStrings:" + settingKey);
        }
    }
}
