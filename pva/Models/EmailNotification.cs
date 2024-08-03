namespace pva.Models
{
    public class EmailNotification
    {
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public string Recipient { get; set; }
    }
}
