using Azure.Messaging.ServiceBus;
using DDAC_Assignment_Mining_Commerce.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace DDAC_Assignment_Mining_Commerce.Services
{
    public class WorkerService : BackgroundService
    {
        private readonly ILogger<WorkerService> _logger;
        private readonly IServiceProvider _services;

        public WorkerService(ILogger<WorkerService> logger, IServiceProvider services)
        {
            _logger = logger;
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using IServiceScope scope = _services.CreateScope();
                IConfiguration configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                RunBuyerNotificationProcessor(configuration);
                break;
            }
        }

        private void RunBuyerNotificationProcessor(IConfiguration configuration)
        {
            string connString = configuration.GetConnectionString("ServiceBusConnection");
            ServiceBusClient client = new ServiceBusClient(connString);
            ServiceBusProcessor processor = client.CreateProcessor("buyer-notification", new ServiceBusProcessorOptions());

            // add handler to process messages
            processor.ProcessMessageAsync += BuyerNotificationMessageHandler;

            // add handler to process any errors
            processor.ProcessErrorAsync += ErrorHandler;

            // start processing 
            _ = processor.StartProcessingAsync();
        }

        private async Task BuyerNotificationMessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();
            Notification notification = JsonSerializer.Deserialize<Notification>(body);
            double? oldPrice = (double?)args.Message.ApplicationProperties["productOldPrice"];
            double? newPrice = (double?)args.Message.ApplicationProperties["productNewPrice"];

            IServiceScope scope = _services.CreateScope();
            MiningCommerceContext context = scope.ServiceProvider.GetRequiredService<MiningCommerceContext>();
            TableService tableService = scope.ServiceProvider.GetRequiredService<TableService>();

            List<Subscription> subscriptions = await tableService.GetSubscriptionsByRK(notification.SellerID.ToString());
            foreach (Subscription subscription in subscriptions)
            {
                SellerModel seller = await context.Seller.FirstOrDefaultAsync(m => m.ID == subscription.sellerID);
                notification.BuyerID = subscription.buyerID;
                notification.GenerateMessage(seller.storeName, notification.Product.productName, oldPrice, newPrice);
                notification.AssignPartitionKey();
                notification.AssignRowKey();

                TableOperation tableOperation = TableOperation.Insert(notification);
                _ = tableService.getTable("notifications").ExecuteAsync(tableOperation);
            }
            await args.CompleteMessageAsync(args.Message);
        }

        private static Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
    }
}
