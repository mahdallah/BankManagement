namespace BankManagement.MobileApp.Models
{
    public class MadaPayPurchase
    {
        public decimal Amount { get; set; }
        public string Card { get; set; }
        public string Merchant { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
