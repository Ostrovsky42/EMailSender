using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using EmailWorker.Models;
using EmailWorker.Service;
using MailExchange;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace EmailWorker
{
    internal class MailConsumer : IConsumer<IMailExchangeModel>
    {
        private readonly ILogger<MailConsumer> _logger;
        private readonly IEMailSenderService _service;
        private EmailDto _dto;

        public MailConsumer(ILogger<MailConsumer> logger, IEMailSenderService service)
        {
            _logger = logger;
            _dto = new EmailDto();
            _service = service;
        }

        public async Task Consume(ConsumeContext<IMailExchangeModel> context)
        {
            _dto.MailAddresses = new List<MailAddress> { new($"{context.Message.MailAddresses}") };
            _dto.Subject = context.Message.Subject;
            _dto.Body = context.Message.Body;
            _dto.DisplayName = context.Message.DisplayName;
            _dto.IsBodyHtml = context.Message.IsBodyHtml;

            _service.SendMail(_dto);
            await Task.CompletedTask;
        }
    }
}