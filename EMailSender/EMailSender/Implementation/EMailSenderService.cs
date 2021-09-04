using System.Net.Mail;
using System.Net;


namespace EMailSender
{
    public class EMailSenderService
    {
        public void SendMail(EmailDto emailDto)         
        {
            MailAddress fromMailAddress = new MailAddress("Test42Test42Test42Test42Test@gmail.com", "CRM");           
            foreach (var toAddress in emailDto.MailAddresses)
            {
                using (MailMessage mailMessage = new MailMessage(fromMailAddress, toAddress))
                using (SmtpClient smtpClient = new SmtpClient())
                {
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
