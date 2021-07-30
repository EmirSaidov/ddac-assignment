using DDAC_Assignment_Mining_Commerce.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
                    var ctx = scope.ServiceProvider.GetRequiredService<TableService>();

                    var subscriptions = await ctx.GetSubscriptionsByPK("1");
                    var count = 1;
                    foreach(Subscription subscription in subscriptions)
                    {
                        Debug.WriteLine(subscription.buyerID);
                        Debug.WriteLine(count);
                        count++;
                    }   
                }
            }
        }
    }
}
