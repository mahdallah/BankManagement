using BankManagement.MobileApp.Models;
using System.Text.RegularExpressions;

namespace BankManagement.MobileApp.Helpers
{
    public class MadaPayPurchaseParser
    {
        public MadaPayPurchase ParseMessage(string body)
        {
            var match = Regex.Match(body,
                @"Amount:\sSAR\s(?<amount>\d+(\.\d{1,2})?)\s+Mada card:\s(?<card>\d{4}\*)\s+At:\s(?<merchant>.+)\s+On:\s(?<date>\d{4}-\d{1,2}-\d{1,2}\s\d{2}:\d{2})");

            if (match.Success)
            {
                return new MadaPayPurchase // This model works for mada pay purchases as well
                {
                    Amount = decimal.Parse(match.Groups["amount"].Value),
                    Card = match.Groups["card"].Value,
                    Merchant = match.Groups["merchant"].Value,
                    TransactionDate = DateTime.Parse(match.Groups["date"].Value)
                };
            }
            else
            {
                throw new ArgumentException("Couldn't parse mada pay purchase");
            }
        }
    }

}
