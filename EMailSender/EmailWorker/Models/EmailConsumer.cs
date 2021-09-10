using System.Collections.Generic;
using System.Net.Mail;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using EmailWorker.Service;
using EventContracts;

namespace EmailWorker.Models
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