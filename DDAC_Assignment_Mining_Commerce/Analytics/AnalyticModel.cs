using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDAC_Assignment_Mining_Commerce.Models.Analytics
{
    public interface AnalyticModel
    {
        //Name of table for analytics
        public string tableName();
        //Operation for Inserting Analytics
        public TableBatchOperation operations();
        
    }
}
