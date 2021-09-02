using System.Collections.Generic;
using System.Net.Mail;

namespace EMailSender
{
    class Program
    {
        static void Main(string[] args)
        {
            EMailSenderService qq = new EMailSenderService();
            List<MailAddress> listMail = new List<MailAddress>();
            listMail.Add(new ("tootoo9723@gmail.com" ));
            listMail.Add(new("Zhekul.90@gmail.com"));
            qq.SendMail("Test", "Dear Student", listMail);
        }
    }
}
