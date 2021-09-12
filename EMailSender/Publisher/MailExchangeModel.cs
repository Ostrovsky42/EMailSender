namespace MailTransaction
{
    public interface MailExchangeModel
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string DisplayName { get; set; }
        public string MailAddresses { get; set; }
    }
}