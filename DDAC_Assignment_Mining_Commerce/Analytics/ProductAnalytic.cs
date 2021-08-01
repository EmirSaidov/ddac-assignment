using DDAC_Assignment_Mining_Commerce.Models.Analytics;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDAC_Assignment_Mining_Commerce.Analytics
{
    public class ProductAnalytic : TableEntity, AnalyticModel
    {
        public ProductAnalytic() { }
        public ProductAnalytic(int product_id,int seller_id)
        {
            this.product_id = product_id;
            this.PartitionKey = seller_id.ToString();
            this.RowKey = product_id.ToString();

        }
        public int product_id { get; set; }
        public DateTime created_at { get; set; } = DateTime.Now;
        TableBatchOperation batch = new TableBatchOperation();

        public string tableName()
        {
            return "NewProduct";
        }

        public TableBatchOperation operations()
        {
            batch.InsertOrReplace(this);
            return batch;
        }
        public string getProductDate()
        {
            return this.created_at.ToString("dd/MM/yy");
        }
    }
}
