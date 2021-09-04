using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
using Serilog;
using Serilog.Formatting.Compact;

namespace EMailSender
{
    public class EMailSenderService
    {
        public void SendMail(string title, string textMessage, List<MailAddress> listMail)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(new CompactJsonFormatter(), "log")
                .CreateLogger();


            var fromMailAddress = new MailAddress("Test42Test42Test42Test42Test@gmail.com", "CRM");
            
            foreach (var email in listMail)
            {
                using (MailMessage mailMessage = new MailMessage(fromMailAddress, email))
                using (SmtpClient smtpClient = new SmtpClient())
                {
                    Log.Debug("Write {Text} with {subject} to {email}", textMessage, title, email);
                    mailMessage.Subject = title;
                    mailMessage.Body = textMessage;
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