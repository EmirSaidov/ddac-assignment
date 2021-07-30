//using Azure.Messaging.ServiceBus;
//using DDAC_Assignment_Mining_Commerce.Models;
//using Microsoft.Extensions.Configuration;
//using Microsoft.ServiceBus;
//using Microsoft.ServiceBus.Messaging;
//using System;
//using System.Collections.Generic;
//using Microsoft.EntityFrameworkCore;
//using System.Diagnostics;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Http;

//namespace DDAC_Assignment_Mining_Commerce.Services
//{
//    public class BusService
//    {
//        private IHttpContextAccessor _httpContextAccessor;
//        private static string connString;
//        string queueName = "seller-notification";
//        ServiceBusClient client;
//        ServiceBusProcessor processor;

//        public BusService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
//        {
//            this._httpContextAccessor = httpContextAccessor;

//            connString = configuration.GetConnectionString("ServiceBusConnection");
//            client = new ServiceBusClient(connString);
//            processor = client.CreateProcessor(queueName, new ServiceBusProcessorOptions());

//            // add handler to process messages
//            processor.ProcessMessageAsync += MessageHandler;

//            // add handler to process any errors
//            processor.ProcessErrorAsync += ErrorHandler;

//            // start processing 
//            _ = processor.StartProcessingAsync();
//        }

//        public async Task QueueProductSaleNotification(ProductModel product)
//        {
//            var sender = client.CreateSender(queueName);
//            using ServiceBusMessageBatch message = await sender.CreateMessageBatchAsync();
//            message.TryAddMessage(new ServiceBusMessage($"Message {product.ID}, {product.sellerID}, {product.imageUri}, {product.productName}, {product.productPrice}, {product.productMass}, {product.productDescription}"));
//            await sender.SendMessagesAsync(message);
//            await sender.DisposeAsync();
//        }

//        public void PostNotification(string notification)
//        {
//            var js = @"
//            <SCRIPT LANGUAGE=""""JavaScript"""">
//                alert(""Hello this is an Alert"")
//            </SCRIPT>
//            ";

//            var context = _httpContextAccessor.HttpContext;
//            Debug.WriteLine(context);
//            _ = context.Response.WriteAsync(js);
//        }

//        async Task MessageHandler(ProcessMessageEventArgs args)
//        {
//            string body = args.Message.Body.ToString();
//            Debug.WriteLine($"Received: {body}");
//            PostNotification(body);
//            // complete the message. messages is deleted from the queue. 
//            await args.CompleteMessageAsync(args.Message);
//        }

//        static Task ErrorHandler(ProcessErrorEventArgs args)
//        {
//            Console.WriteLine(args.Exception.ToString());
//            return Task.CompletedTask;
//        }
//    }
//}
