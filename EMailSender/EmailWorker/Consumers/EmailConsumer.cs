using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using EmailWorker.Models;
using EmailWorker.Service;
using EventContracts;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace EmailWorker.Consumers
{
    class EmailConsumer : IConsumer<QueueMailTransaction>
    {
        ILogger<EmailConsumer> _logger;
        private readonly IEMailSenderService _service;
        private EmailDto _dto;

        public EmailConsumer(ILogger<EmailConsumer> logger, IEMailSenderService service)
        {
            _logger = logger;
            _dto = new EmailDto();
            _service = service;
        }

        public async Task Consume(ConsumeContext<QueueMailTransaction> context)
        {
            _dto.MailAddresses = new List<MailAddress> {new($"{context.Message.MailAddresses}")};
            _dto.Amount = context.Message.Amount;
            _service.SendMail(_dto);

            _logger.LogInformation("Value: {Value}", context.Message);

            await Task.CompletedTask;
        }
    }
}