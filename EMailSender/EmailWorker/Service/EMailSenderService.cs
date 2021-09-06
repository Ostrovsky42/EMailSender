using System.Net;
using System.Net.Mail;
using EmailWorker.Models;
using EmailWorker.Settings;
using Microsoft.Extensions.Options;

namespace EmailWorker.Service
{
    public class EMailSenderService : IEMailSenderService
    {
        public void SendMail(EmailDto emailDto, IOptions<ConnSettings> options)
        {
            //Log.Logger = new LoggerConfiguration()
            //    .MinimumLevel.Debug()
            //    .WriteTo.File(new CompactJsonFormatter(), "logger") //.WriteTo.Seq("https://80.78.240.16/logger.txt")
            //    .CreateLogger();

            var fromMailAddress = new MailAddress(options.Value.FromEmail, options.Value.DisplayName);
            foreach (var toAddress in emailDto.MailAddresses)
            {
                using (var mailMessage = new MailMessage(fromMailAddress, toAddress))
                using (var smtpClient = new SmtpClient())
                {
                    //Log.Debug($"Write body:[{emailDto.Body}] with Subject:[{emailDto.Subject}] to [{toAddress}] from [{fromMailAddress.Address}]");

                    mailMessage.Subject = emailDto.Subject;
                    mailMessage.Body = emailDto.Body;
                    smtpClient.Host = options.Value.Host;
                    smtpClient.Port = options.Value.Port;
                    smtpClient.EnableSsl = true;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(fromMailAddress.Address, options.Value.Password);
                    smtpClient.Send(mailMessage);
                }
            }
        }
    }
}