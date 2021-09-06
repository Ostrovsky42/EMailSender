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

        public Worker(ILogger<Worker> logger, IOptions<EmailConfig> options)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options ?? throw new ArgumentNullException(nameof(options));
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
                        MailAddresses = new List<MailAddress> {new MailAddress("zhekul.90@gmail.com")}
                    };

                    var service = new EMailSenderService(_options, _logger);
                    service.SendMail(dto);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
