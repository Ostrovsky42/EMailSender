using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace EmailWorker
{
    public class Worker : BackgroundService
    {
        private int _timeSpan = 360000;

        public Worker() { }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            Log.Information($"Worker started at: {DateTime.Now}");
            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            Log.Information($"Worker stopped at: {DateTime.Now}");
            await base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(_timeSpan, stoppingToken);
            }
        }
    }
}