using System;
using System.Net;
using System.Net.Mail;
using EmailWorker.Models;
using EmailWorker.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EmailWorker.Service
{
    public class EMailSenderService : IEMailSenderService
    {
        private readonly EmailConfig _config;
        private readonly ILogger<Worker> _logger;

        public EMailSenderService(IOptions<EmailConfig> options, ILogger<Worker> logger)
        {
            _config = options.Value;
            _logger = logger;
        }

        public void SendMail(EmailDto emailDto)
        {
            var fromMailAddress = new MailAddress(_config.FromMailAddress, emailDto.DisplayName);
            foreach (var toAddress in emailDto.MailAddresses)
            {
                using (var mailMessage = new MailMessage(fromMailAddress, toAddress))
                using (var smtpClient = new SmtpClient())
                {
                    mailMessage.Subject = emailDto.Subject;
                    mailMessage.Body = emailDto.Body;
                    smtpClient.Host = _config.Host;
                    smtpClient.Port = _config.Port;
                    smtpClient.EnableSsl = true;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(fromMailAddress.Address, _config.Password);
                    
                    smtpClient.Send(mailMessage);

                    _logger.LogInformation($"Send(mailMessage) to [{toAddress}] from [{fromMailAddress.Address}], body:[{emailDto.Body}] with Subject:[{emailDto.Subject}] at {DateTimeOffset.Now}");
                }
            }
        }
    }
}