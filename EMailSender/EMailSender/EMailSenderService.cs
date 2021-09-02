using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;

namespace EMailSender
{
    public class EMailSenderService
    {
        public void SendMail(string title, string textMessage, List<MailAddress> listMail)
        {
            MailAddress fromMailAddress = new MailAddress("Test42Test42Test42Test42Test@gmail.com", "CRM");
            MailAddress toAddress = new MailAddress("tootoo9723@gmail.com", "Uncle Bob");
            foreach (var email in listMail)
            {
                using (MailMessage mailMessage = new MailMessage(fromMailAddress, email))
                using (SmtpClient smtpClient = new SmtpClient())
                {
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
