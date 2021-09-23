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
                    AlternateView alterViewImage = ContentToAlterView.ContentToAlternateView(emailDto.Base64String);
                    AlternateView alterViewBody = ContentToAlterView.ContentToAlternateView(emailDto.Body);
                    mailMessage.AlternateViews.Add(alterViewImage);
                    mailMessage.AlternateViews.Add(alterViewBody);                   
                    mailMessage.IsBodyHtml = emailDto.IsBodyHtml;
                    smtpClient.Host = _config.Host;
                    smtpClient.Port = _config.Port;
                    smtpClient.EnableSsl = true;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(fromMail.Address, _config.Password);
                    SmtpClient smtp = new SmtpClient(_config.Host, _config.Port) { EnableSsl = false };
                    await smtpClient.SendMailAsync(mailMessage);
                    Log.Information($"{DateTimeOffset.Now} Send(mailMessage) to [{toMail}] from [{fromMail.Address}], body:[{emailDto.Body}] with Subject:[{emailDto.Subject}]");
                }
            }
        }
    }
}