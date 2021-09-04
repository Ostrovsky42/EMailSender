using System.Collections.Generic;
using System.Net.Mail;

namespace EMailSender
{
    public class EmailDto   
    {       
        public string Subject { get; set; }
        public string Body { get; set; }
        public string From { get; set; }
        public List<MailAddress> MailAddresses { get; set; }
        public bool IsBodyHtml { get; set; }
    }
}