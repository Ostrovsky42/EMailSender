namespace EmailWorker.Settings
{
    public class EmailConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string FromMailAddress { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
    }
}