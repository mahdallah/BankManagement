namespace BankManagement.MobileApp.Models
{
    public class OnlinePurchase
    {
        public decimal Amount { get; set; }
        public string Card { get; set; }
        public string Account { get; set; }
        public string Merchant { get; set; }
        public string Location { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
