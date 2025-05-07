using BankManagement.MobileApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BankManagement.MobileApp.Helpers
{
    public class OnlinePurchaseParser
    {
        public OnlinePurchase ParseMessage(string body)
        {
            // Simplified regex without the optional location line
            var matchWithoutLocation = Regex.Match(body,
        @"Amount:\s(?<amount>\d+(\.\d{1,2})?)\sSAR\s+mada card:\s(?<card>\d{4}\*)\s+From Account:\s(?<account>\*\d{4})\s+at:\s(?<merchant>.+?)\s+On:\s(?<date>\d{4}-\d{1,2}-\d{1,2}\s\d{2}:\d{2})",
        RegexOptions.IgnoreCase);

            if (matchWithoutLocation.Success)
            {
                return new OnlinePurchase
                {
                    Amount = decimal.Parse(matchWithoutLocation.Groups["amount"].Value),
                    Card = matchWithoutLocation.Groups["card"].Value,
                    Account = matchWithoutLocation.Groups["account"].Value,
                    Merchant = matchWithoutLocation.Groups["merchant"].Value,
                    TransactionDate = DateTime.Parse(matchWithoutLocation.Groups["date"].Value)
                };
            }

            // Combined regex for both cases (with and without location)
            var matchWithLocation = Regex.Match(body,
                @"Amount:\s(?<amount>\d+(\.\d{1,2})?)\sSAR\s+mada card:\s(?<card>\d{4}\*)\s+From Account:\s(?<account>\*\d{4})\s+At:\s(?<merchant>.+?)\s+In:\s(?<location>.+?)\s+On:\s(?<date>\d{4}-\d{1,2}-\d{1,2}\s\d{2}:\d{2})",
                RegexOptions.IgnoreCase);

            if (matchWithLocation.Success)
            {

                return new OnlinePurchase
                {
                    Amount = decimal.Parse(matchWithLocation.Groups["amount"].Value),
                    Card = matchWithLocation.Groups["card"].Value,
                    Account = matchWithLocation.Groups["account"].Value,
                    Merchant = matchWithLocation.Groups["merchant"].Value,
                    Location = matchWithLocation.Groups["location"].Value,
                    TransactionDate = DateTime.Parse(matchWithLocation.Groups["date"].Value)
                };
            }


            var lines = body.Split('\n');
            var onlinePurchase = new OnlinePurchase();
            foreach (var line in lines)
            {
                if (line.Contains("Amount:"))
                {
                    onlinePurchase.Amount = decimal.Parse(line.Split(' ')[1]);
                }
                if (line.Contains("mada card:"))
                {
                    onlinePurchase.Card = line.Split(' ')[2];
                }
                if (line.Contains("From Account:"))
                {
                    onlinePurchase.Account = line.Split(' ')[2];
                }
                if (line.Contains("at:"))
                {
                    onlinePurchase.Merchant = line.Split(' ')[1];
                }
                if (line.Contains("In:"))
                {
                    onlinePurchase.Location = line.Split(' ')[1];
                }
                if (line.Contains("On:"))
                {
                    onlinePurchase.TransactionDate = DateTime.Parse(line.Split(' ')[1]);
                }
                if (line.Contains("Time:"))
                {
                    onlinePurchase.TransactionDate = onlinePurchase.TransactionDate.Add(TimeSpan.Parse(line.Split(' ')[1]));
                }
            }

            return onlinePurchase;
        }
    }

}
