using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using EmailWorker.Settings;
using Microsoft.Extensions.Options;
using Serilog;

namespace EmailWorker
{
    public class Worker : BackgroundService
    {
        private readonly IOptions<EmailConfig> _options;
        private int _timeSpan = 360000;

        public Worker(IOptions<EmailConfig> options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

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
                Log.Information("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(_timeSpan, stoppingToken);
            }
        }
    }
}