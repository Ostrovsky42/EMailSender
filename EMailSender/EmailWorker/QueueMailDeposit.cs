namespace EventContracts
{
    public interface QueueMailDeposit
    {
        public string Amount { get; set; }
        public string MailAddresses { get; set; }
    }
}
