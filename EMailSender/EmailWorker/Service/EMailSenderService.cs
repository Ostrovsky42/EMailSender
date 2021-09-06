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

        public EMailSenderService(IOptions<EmailConfig> config, ILogger<Worker> logger)
        {
            _config = config.Value;
            _logger = logger;
        }

        public void SendMail(EmailDto emailDto)
        {
            //Log.Logger = new LoggerConfiguration()
            //    .MinimumLevel.Debug()
            //    .WriteTo.File(new CompactJsonFormatter(), "logger") //.WriteTo.Seq("https://80.78.240.16/logger.txt")
            //    .CreateLogger();

            var fromMailAddress = new MailAddress(_config.FromEmail, _config.DisplayName);
            foreach (var toAddress in emailDto.MailAddresses)
            {
                using (var mailMessage = new MailMessage(fromMailAddress, toAddress))
                using (var smtpClient = new SmtpClient())
                {
                    //Log.Debug($"Write body:[{emailDto.Body}] with Subject:[{emailDto.Subject}] to [{toAddress}] from [{fromMailAddress.Address}]");

                    mailMessage.Subject = emailDto.Subject;
                    mailMessage.Body = emailDto.Body;
                    smtpClient.Host = _config.Host;
                    smtpClient.Port = _config.Port;
                    smtpClient.EnableSsl = true;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(fromMailAddress.Address, _config.Password);
                    smtpClient.Send(mailMessage);
                }
            }
        }
    }
}