using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDAC_Assignment_Mining_Commerce.Models
{
    public class Notification : TableEntity
    {
        public int SellerID { get; set; }
        public int BuyerID { get; set; }
        public int ProductID { get; set; }
        public ProductModel Product { get; set; }
        public NotificationType Type { get; set; }
        public string Message { get; set; }

        public void AssignRowKey()
        {
            this.RowKey = Guid.NewGuid().ToString();
        }
        public void AssignPartitionKey()
        {
            this.PartitionKey = BuyerID.ToString();
        }

        public void GenerateMessage(string storeName, string productName, double? productOldPrice, double? productNewPrice)
        {
            switch (Type)
            {
                case NotificationType.NewProduct:
                    Message = string.Format("{0} has added a new product: {1}.", storeName, productName); ;
                    break;
                case NotificationType.EditProductPrice:
                    Message = string.Format("{0} has changed {1}'s price from {2} to {3}.", storeName, productName, productOldPrice, productNewPrice); ;
                    break;
                case NotificationType.RemoveProduct:
                    Message = string.Format("{0} has removed the product: {1}.", storeName, productName); ;
                    break;
                default:
                    Message = "";
                    break;
            }
        }
    }

    public enum NotificationType
    {
        NewProduct,
        EditProductPrice,
        RemoveProduct,
    }

    public class NotificationList
    {
        public int SelectedNotificationId { get; set; }
        public IEnumerable<Notification> Notifications { get; set; }
    }
}
