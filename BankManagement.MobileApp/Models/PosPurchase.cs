namespace BankManagement.MobileApp.Models
{
    public class PosPurchase
    {
        public string PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public string Card { get; set; }
        public string Account { get; set; }
        public string Merchant { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
