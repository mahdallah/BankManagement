using BankManagement.MobileApp.Models;
using System.Globalization;
using System.Text.RegularExpressions;

namespace BankManagement.MobileApp.Helpers
{
    public class PosPurchaseParser
    {
        public PosPurchase ParseMessage(string body)
        {
            var match = Regex.Match(body,
                @"By:\s(?<paymentMethod>.+)\s+Amount:\s(?<amount>\d+(\.\d{1,2})?)\sSAR\s+mada card:\s(?<card>\d{4}\*)\s+Account:\s(?<account>\*\*\d{4})\s+At:\s(?<merchant>.+)\s+On:\s(?<date>\d{4}-\d{1,2}-\d{1,2}\s\d{2}:\d{2})");

            if (match.Success)
            {
                return new PosPurchase
                {
                    PaymentMethod = match.Groups["paymentMethod"].Value,
                    Amount = decimal.Parse(match.Groups["amount"].Value),
                    Card = match.Groups["card"].Value,
                    Account = match.Groups["account"].Value,
                    Merchant = match.Groups["merchant"].Value,
                    TransactionDate = DateTime.Parse(match.Groups["date"].Value)
                };
            }

            var lines = body.Split('\n');
            var posPurchase = new PosPurchase();
            foreach (var line in lines)
            {
                if (line.Contains("By:"))
                {
                    posPurchase.PaymentMethod = line.Split(':')[1];
                }
                if (line.Contains("Amount:"))
                {
                    var amountAndCurrency = line.Split(':')[1].Split(' ');
                    var canParse = decimal.TryParse(amountAndCurrency[1], out var amount);
                    posPurchase.Amount = amount;
                }
                if (line.Contains("mada card:"))
                {
                    posPurchase.Card = line.Split(' ')[2];
                }
                if (line.Contains("Account:"))
                {
                    posPurchase.Account = line.Split(' ')[1];
                }
                if (line.Contains("At:"))
                {
                    posPurchase.Merchant = line.Split(' ')[1];
                }
                if (line.Contains("On:"))
                {
                    // Extract the part after "On: "
                    string dateTimeString = line.Substring(4).Trim();

                    // Define the format
                    string format = "dd-MM-yy HH:mm";

                    // Parse the date and time
                    var transactionDate = DateTime.ParseExact(dateTimeString, format, CultureInfo.InvariantCulture);

                    posPurchase.TransactionDate = transactionDate;
                }
                if (line.Contains("Time:"))
                {
                    posPurchase.TransactionDate = posPurchase.TransactionDate.Add(TimeSpan.Parse(line.Split(' ')[1]));
                }
            }

            return posPurchase;
        }
    }

}
