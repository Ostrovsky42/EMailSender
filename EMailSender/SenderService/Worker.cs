using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SenderService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json");

            Configuration = builder.Build();
            Configuration.SetEnvironmentVariableForConfiguration();
        }
        public IConfiguration Configuration { get; }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
