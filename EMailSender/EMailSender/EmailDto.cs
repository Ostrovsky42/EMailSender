namespace EMailSender
{
    public class EmailDto   
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public bool IsBodyHtml { get; set; }
    }
}