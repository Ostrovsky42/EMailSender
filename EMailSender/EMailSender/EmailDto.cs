using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMailSender
{
    public class EmailDto   
    {
        public string Attachments { get; set; }
        public string Body { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public bool IsBodyHtml { get; set; }

    }
}
