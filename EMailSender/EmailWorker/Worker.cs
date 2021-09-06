using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using EmailWorker.Models;
using EmailWorker.Service;
using EmailWorker.Settings;
using Microsoft.Extensions.Options;

namespace EmailWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IOptions<EmailConfig> _options;
        private int _timeSpan = 5000;

        public Worker(ILogger<Worker> logger, IOptions<EmailConfig> options)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Worker started at: {DateTime.Now}");
            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Worker stopped at: {DateTime.Now}");
            await base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                try
                {
                    var dto = new EmailDto
                    {
                        Subject = "Test",
                        Body = "Come over here!",
                        MailAddresses = new List<MailAddress> { new("zhekul.90@gmail.com") }
                    };

                    var service = new EMailSenderService(_options, _logger);
                    service.SendMail(dto);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    _logger.LogError("throw Exception {e}", e);
                }

                await Task.Delay(_timeSpan, stoppingToken);
            }
        }
    }
}
