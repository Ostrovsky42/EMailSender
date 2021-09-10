namespace MailTransaction
{
    public interface MailTransactionExchangeModel
    {
        public string Amount { get; set; }
        public string MailAddresses { get; set; }
    }
}
