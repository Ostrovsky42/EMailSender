using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using EmailWorker.Models;
using EmailWorker.Service;
using MailTransaction;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace EmailWorker.Consumers
{
    class EmailConsumer : IConsumer<MailTransactionExchangeModel>
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

        public async Task Consume(ConsumeContext<MailTransactionExchangeModel> context)
        {
            _logger.LogInformation("Value: {Value}", context.Message);

            _dto.MailAddresses = new List<MailAddress> {new($"{context.Message.MailAddresses}")};
            _dto.Amount = context.Message.Amount;
            _service.SendMail(_dto);

            await Task.CompletedTask;
        }
    }
}