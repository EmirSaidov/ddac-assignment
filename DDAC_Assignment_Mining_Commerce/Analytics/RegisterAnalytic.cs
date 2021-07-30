using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDAC_Assignment_Mining_Commerce.Models.Analytics
{

    public class RegisterAnalytic : TableEntity, AnalyticModel
    {
        public RegisterAnalytic() { }
        public RegisterAnalytic(int user_id,string role)
        {
            this.user_id = user_id;
            this.month = DateTime.Now.ToString("MM");
            this.year = DateTime.Now.ToString("yy");
            this.PartitionKey = this.month + ":" + this.year;
            this.RowKey = this.user_id.ToString();
        }
        public int user_id { get; set; }
        public string role { get; set; }
        public string month;
        public string year;
        TableBatchOperation batch = new TableBatchOperation();

        public string tableName()
        {
            return "Registration";
        }

        public TableBatchOperation operations()
        {
            batch.InsertOrReplace(this);
            return batch;
        }
    }
}
