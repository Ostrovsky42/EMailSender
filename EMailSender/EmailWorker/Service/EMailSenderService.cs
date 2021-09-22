using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using EmailWorker.Models;
using EmailWorker.Settings;
using Microsoft.Extensions.Options;
using Serilog;


namespace EmailWorker.Service
{
    public class EMailSenderService : IEMailSenderService
    {
        private readonly EmailConfig _config;

        public EMailSenderService(IOptions<EmailConfig> options)
        {
            _config = options.Value ?? throw new Exception("options.Value was null");
        }

        public async Task SendMailAsync(EmailDto emailDto)
        {
            var fromMail = new MailAddress(_config.FromMailAddress, emailDto.DisplayName);
            foreach (var toMail in emailDto.MailAddresses)
            {
                using (var mailMessage = new MailMessage(fromMail, toMail))
                using (var smtpClient = new SmtpClient())
                {
                    mailMessage.Subject = emailDto.Subject;
                    mailMessage.Body = emailDto.Body;
                    mailMessage.IsBodyHtml = emailDto.IsBodyHtml;
                    AlternateView alterView = ContentToAlterView.ContentToAlternateView(emailDto.Base64String);
                    mailMessage.AlternateViews.Add(alterView);
                    smtpClient.Host = _config.Host;
                    smtpClient.Port = _config.Port;
                    smtpClient.EnableSsl = true;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(fromMail.Address, _config.Password);
                    
                    smtpClient.Send(mailMessage);
                    Log.Information($"{DateTimeOffset.Now} Send(mailMessage) to [{toMail}] from [{fromMail.Address}], body:[{emailDto.Body}] with Subject:[{emailDto.Subject}]");
                }
            }
        }
    }
}