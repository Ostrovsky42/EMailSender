﻿using System.Net;
using System.Net.Mail;
using EmailWorker.Models;

namespace EmailWorker.Service
{
    public class EMailSenderService : IEMailSenderService   
    {
        public void SendMail(EmailDto emailDto)
        {
            //Log.Logger = new LoggerConfiguration()
            //    .MinimumLevel.Debug()
            //    .WriteTo.File(new CompactJsonFormatter(), "logger") //.WriteTo.Seq("https://80.78.240.16/logger.txt")
            //    .CreateLogger();

            var fromMailAddress = new MailAddress("Test42Test42Test42Test42Test@gmail.com", "CRM");
            foreach (var toAddress in emailDto.MailAddresses)
            {
                using (var mailMessage = new MailMessage(fromMailAddress, toAddress))
                using (var smtpClient = new SmtpClient())
                {
                    //Log.Debug($"Write body:[{emailDto.Body}] with Subject:[{emailDto.Subject}] to [{toAddress}] from [{fromMailAddress.Address}]");

                    mailMessage.Subject = emailDto.Subject;
                    mailMessage.Body = emailDto.Body;
                    smtpClient.Host = "smtp.gmail.com";
                    smtpClient.Port = 587;
                    smtpClient.EnableSsl = true;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(fromMailAddress.Address, "EMailSenderTest42");
                    smtpClient.Send(mailMessage);
                }
            }
        }
    }
}