namespace BankManagement.MobileApp
{
    public class SmsMessage
    {
        public int MessageId { get; set; }  // Unique message ID
        public int ThreadId { get; set; }   // Conversation thread ID
        public string Sender { get; set; }  // Sender's phone number
        public string ContactId { get; set; }  // Contact ID (if saved)
        public DateTime DateReceived { get; set; } // When the message was received
        public DateTime DateSent { get; set; }  // When the message was sent
        public string Message { get; set; }  // SMS content
        public string MessageType { get; set; } // Inbox, Sent, etc.
        public string ReadStatus { get; set; }  // Read or Unread
        public string Status { get; set; }  // Sent/Received status
        public string ServiceCenter { get; set; }  // SMS Service Center
        public bool IsLocked { get; set; }  // Locked status
        public int ErrorCode { get; set; }  // Error Code (if any)
    }
    public class SmsSenderViewDto
    {
        public string Address { get; set; }
    }
}
