using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDAC_Assignment_Mining_Commerce.Models.Analytics
{

    public class LoginAnalytic : TableEntity, AnalyticModel
    {
        public LoginAnalytic() { }
        public LoginAnalytic(int user_id)
        {
            this.user_id = user_id;
            this.month = DateTime.Now.ToString("MM");
            this.year = DateTime.Now.ToString("yy");
            this.PartitionKey = this.month + ":" + this.year;
            this.RowKey = this.user_id + ":" + this.Timestamp.ToString("dd") + ":" + this.month +":" + this.year;
        }
        public int user_id { get; set; }
        public string month;
        public string year;
        TableBatchOperation batch = new TableBatchOperation();
        public string tableName()
        {
            return "Login";
        }

        public TableBatchOperation operations()
        {
            batch.InsertOrReplace(new LoginAnalytic(this.user_id));
            return batch;
        }
    }
}
