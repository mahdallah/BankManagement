namespace BankManagement.MobileApp.Models
{
    public class TransferBetweenAccounts
    {
        public string DebitAccount { get; set; }
        public string CreditAccount { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
