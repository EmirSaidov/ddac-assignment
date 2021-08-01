using DDAC_Assignment_Mining_Commerce.Models.Analytics;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDAC_Assignment_Mining_Commerce.Analytics
{
    public class OrderAnalytic : TableEntity, AnalyticModel
    {
        public OrderAnalytic() { }
        public OrderAnalytic(int buyer_id, int product_count, double total_amount)
        {
            this.buyer_id = buyer_id;
            this.product_count = product_count;
            this.total_amount = total_amount;
            this.PartitionKey = buyer_id.ToString();
            this.RowKey = buyer_id.ToString() + created_at.ToString("mm:HH:dd:MM:yyyy");

        }
        public int buyer_id { get; set; }
        public int product_count { get; set; }
        public double total_amount { get; set; }
        public DateTime created_at { get; set; } = DateTime.Now;
        TableBatchOperation batch = new TableBatchOperation();

        public string tableName()
        {
            return "Order";
        }

        public TableBatchOperation operations()
        {
            batch.InsertOrReplace(this);
            return batch;
        }
        public string getOrderDate()
        {
            return this.created_at.ToString("dd/MM/yy");
        }
    }
}
