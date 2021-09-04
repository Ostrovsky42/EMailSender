using System.Collections.Generic;
using System.Net.Mail;

namespace EMailSender
{
    class Program
    {
        static void Main(string[] args)
        {
            var emailDto = new EmailDto();
            emailDto.MailAddresses = new List<MailAddress>
            {
               // new("tootoo9723@gmail.com"),
                new("Zhekul.90@gmail.com")
            };
            emailDto.Subject = "Test Log";
            emailDto.Body = "Where is my money?";

            var emailSenderService = new EMailSenderService();
            emailSenderService.SendMail(emailDto);
        }
    }
}