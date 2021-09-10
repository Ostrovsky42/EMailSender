namespace EventContracts
{
    public interface QueueMailTransaction
    {
        public string Amount { get; set; }
        public string MailAddresses { get; set; }
    }
}
