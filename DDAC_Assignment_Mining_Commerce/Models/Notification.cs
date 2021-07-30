using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDAC_Assignment_Mining_Commerce.Models
{
    public class Notification : TableEntity
    {
        public int sellerID { get; set; }
        public int buyerID { get; set; }
        public ProductModel product { get; set; }
        public NotificationType type { get; set; }
        public string message { get; set; }

        public void AssignRowKey()
        {
            this.RowKey = sellerID.ToString();
        }
        public void AssignPartitionKey()
        {
            this.PartitionKey = buyerID.ToString();
        }
    }

    public enum NotificationType
    {
        NewProduct
    }
}
