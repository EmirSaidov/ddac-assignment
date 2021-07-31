using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDAC_Assignment_Mining_Commerce.Models
{
    public class Subscription: TableEntity
    {
        public int sellerID { get; set; }
        public int buyerID { get; set; }

        public void AssignRowKey()
        {
            this.RowKey = sellerID.ToString();
        }
        public void AssignPartitionKey()
        {
            this.PartitionKey = buyerID.ToString();
        }
    }
}
