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
    class MailConsumer : IConsumer<MailExchangeModel>
    {
        ILogger<MailConsumer> _logger;
        private readonly IEMailSenderService _service;
        private EmailDto _dto;

        public MailConsumer(ILogger<MailConsumer> logger, IEMailSenderService service)
        {
            _logger = logger;
            _dto = new EmailDto();
            _service = service;
        }

        public async Task Consume(ConsumeContext<MailExchangeModel> context)
        {
            _logger.LogInformation($"context.MailAddresses: [{context.Message.MailAddresses}], " +
                                   $"context.Amount: [{context.Message.Amount}], ");
            
            _dto.MailAddresses = new List<MailAddress> {new($"{context.Message.MailAddresses}")};
            
            _service.SendMail(_dto);
            await Task.CompletedTask;
        }
    }
}