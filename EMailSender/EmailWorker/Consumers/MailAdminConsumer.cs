using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using EmailWorker.Models;
using EmailWorker.Service;
using MailAdmin;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace EmailWorker.Consumers
{
    class MailAdminConsumer : IConsumer<MailAdminExchangeModel>
    {
        ILogger<MailTransactionConsumer> _logger;
        private readonly IEMailSenderService _service;
        private EmailDto _dto;

        public MailAdminConsumer(ILogger<MailTransactionConsumer> logger, IEMailSenderService service)
        {
            _logger = logger;
            _dto = new EmailDto();
            _service = service;
        }

        public async Task Consume(ConsumeContext<MailAdminExchangeModel> context)
        {
            _logger.LogInformation("MailTo: [{m}], Subject: [{s}], Body: [{b}], ", 
                context.Message.MailTo, context.Message.Subject, context.Message.Body);

            _dto.MailAddresses = new List<MailAddress> { new($"{context.Message.MailTo}") };
            _dto.Subject = context.Message.Subject;
            _dto.Body = context.Message.Body;

            _service.SendMail(_dto);

            await Task.CompletedTask;
        }
    }
}