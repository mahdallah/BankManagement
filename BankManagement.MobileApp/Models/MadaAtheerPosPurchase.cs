namespace BankManagement.MobileApp.Models
{
    public class MadaAtheerPosPurchase
    {
        public decimal Amount { get; set; }
        public string Card { get; set; }
        public string Account { get; set; }
        public string Merchant { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
