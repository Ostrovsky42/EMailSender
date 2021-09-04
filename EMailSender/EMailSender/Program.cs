using System.Collections.Generic;
using System.Net.Mail;

namespace EMailSender
{
    class Program
    {
        static void Main(string[] args)
        {      
            EMailSenderService qq = new EMailSenderService();
            EmailDto emailDto =new EmailDto();

            emailDto.MailAddresses.Add(new("tootoo9723@gmail.com"));
            emailDto.MailAddresses.Add(new("Zhekul.90@gmail.com"));
            emailDto.Subject = "Test";
            emailDto.Body = "Where is my money?";
            qq.SendMail(emailDto);          

        }
    }
}
