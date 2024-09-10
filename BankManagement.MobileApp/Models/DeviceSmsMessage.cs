using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public class MadaAtheerPosPurchase
    {
        public decimal Amount { get; set; }
        public string Card { get; set; }
        public string Account { get; set; }
        public string Merchant { get; set; }
        public DateTime TransactionDate { get; set; }
    }

    public class TransferBetweenAccounts
    {
        public string DebitAccount { get; set; }
        public string CreditAccount { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
    }

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

    public class PosPurchase
    {
        public string PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public string Card { get; set; }
        public string Account { get; set; }
        public string Merchant { get; set; }
        public DateTime TransactionDate { get; set; }
    }

    public class MadaPayPurchase
    {
        public decimal Amount { get; set; }
        public string Card { get; set; }
        public string Merchant { get; set; }
        public DateTime TransactionDate { get; set; }
    }
    public class UnknownTemplate
    {
        public string RawMessage { get; set; }
    }
}
