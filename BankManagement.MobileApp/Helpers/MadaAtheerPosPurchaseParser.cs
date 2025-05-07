using BankManagement.MobileApp.Models;
using System.Text.RegularExpressions;

namespace BankManagement.MobileApp.Helpers
{
    public class MadaAtheerPosPurchaseParser
    {
        public MadaAtheerPosPurchase ParseMessage(string body)
        {
            var match = Regex.Match(body,
                @"Amount:\s(?<amount>\d+(\.\d{1,2})?)\sSAR\s+mada card:\s(?<card>\d{4}\*)\s+Account:\s(?<account>\*\*\d{4})\s+At:\s(?<merchant>.+)\s+On:\s(?<date>\d{4}-\d{1,2}-\d{1,2}\s\d{2}:\d{2})");

            if (match.Success)
            {
                return new MadaAtheerPosPurchase
                {
                    Amount = decimal.Parse(match.Groups["amount"].Value),
                    Card = match.Groups["card"].Value,
                    Account = match.Groups["account"].Value,
                    Merchant = match.Groups["merchant"].Value,
                    TransactionDate = DateTime.Parse(match.Groups["date"].Value)
                };
            }
            else
            {
                throw new ArgumentException("Couldn't parse mada atheer pos purchase");
            }
        }
    }

}
