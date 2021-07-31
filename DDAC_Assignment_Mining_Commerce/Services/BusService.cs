using Azure.Messaging.ServiceBus;
using DDAC_Assignment_Mining_Commerce.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Text.Json;
using System;

namespace DDAC_Assignment_Mining_Commerce.Services
{
    public class BusService
    {
        private static string connString;
        private readonly ServiceBusClient client;

        public BusService(IConfiguration configuration)
        {
            connString = configuration.GetConnectionString("ServiceBusConnection");
            client = new ServiceBusClient(connString);
        }

        public async Task QueueNewProductNotification(NotificationType notificationType, ProductModel product, double? productOldPrice, double? productNewPrice)
        {
            ServiceBusSender sender = client.CreateSender("buyer-notification");
            Notification notification = new Notification
            {
                SellerID = product.sellerID,
                ProductID = product.ID,
                Product = product,
                Type = notificationType,
            };

            ServiceBusMessage message = new ServiceBusMessage(JsonSerializer.Serialize(notification));
            if (productOldPrice != null && productNewPrice != null)
            {
                message.ApplicationProperties.Add("productOldPrice", productOldPrice);
                message.ApplicationProperties.Add("productNewPrice", productNewPrice);
            }

            await sender.SendMessageAsync(message);
            await sender.DisposeAsync();
        }
    }
}
