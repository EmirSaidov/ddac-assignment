using Azure.Messaging.ServiceBus;
using DDAC_Assignment_Mining_Commerce.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace DDAC_Assignment_Mining_Commerce.Services
{
    public class BusService
    {
        private static string connString;
        ServiceBusClient client;

        public BusService(IConfiguration configuration)
        {
            connString = configuration.GetConnectionString("ServiceBusConnection");
            client = new ServiceBusClient(connString);
        }

        public async Task QueueNewProductNotification(ProductModel product)
        {
            var sender = client.CreateSender("buyer-notification");
            Notification notification = new Notification
            {
                sellerID = product.sellerID,
                product = product,
                type = NotificationType.NewProduct,
            };
            using ServiceBusMessageBatch message = await sender.CreateMessageBatchAsync();
            message.TryAddMessage(new ServiceBusMessage(JsonSerializer.Serialize(notification)));
            await sender.SendMessagesAsync(message);
            await sender.DisposeAsync();
        }

        async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();
            await args.CompleteMessageAsync(args.Message);
        }

        static Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
    }
}
