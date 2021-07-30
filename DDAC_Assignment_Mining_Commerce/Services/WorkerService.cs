using Azure.Messaging.ServiceBus;
using DDAC_Assignment_Mining_Commerce.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
                using (var scope = _services.CreateScope())
                {
                    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                    RunBuyerNotificationProcessor(configuration);
                }
                break;
            }
        }

        private void RunBuyerNotificationProcessor(IConfiguration configuration)
        {
            var connString = configuration.GetConnectionString("ServiceBusConnection");
            var client = new ServiceBusClient(connString);
            var processor = client.CreateProcessor("buyer-notification", new ServiceBusProcessorOptions());

            // add handler to process messages
            processor.ProcessMessageAsync += BuyerNotificationMessageHandler;

            // add handler to process any errors
            processor.ProcessErrorAsync += ErrorHandler;

            // start processing 
            _ = processor.StartProcessingAsync();
        }

        async Task BuyerNotificationMessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();
            Notification notification = JsonSerializer.Deserialize<Notification>(body);
            await args.CompleteMessageAsync(args.Message);
        }

        static Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
    }
}
