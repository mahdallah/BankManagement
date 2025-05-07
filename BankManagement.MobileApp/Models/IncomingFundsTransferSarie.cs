namespace BankManagement.MobileApp.Models
{
    public class IncomingFundsTransferSarie
    {
        public string ToAccount { get; set; }
        public decimal Amount { get; set; }
        public string From { get; set; }
        public string FromAccount { get; set; }
        public string FromBank { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Reference { get; set; }
    }
}
