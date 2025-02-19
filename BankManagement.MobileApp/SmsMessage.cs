namespace BankManagement.MobileApp
{
    public class SmsMessage
    {
        public string Address { get; set; }
        public string Body { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; } // "Sent" or "Received"
    }
}
