using System.Collections.Generic;
using System.Net.Mail;

namespace EmailWorker.Models
{
    public class EmailDto
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string DisplayName { get; set; }
        public List<MailAddress> MailAddresses { get; set; }
        public bool IsBodyHtml { get; set; }
    }
}