using BankManagement.MobileApp.Models;
using System.Text.RegularExpressions;

namespace BankManagement.MobileApp.Helpers
{
    public class TransferBetweenAccountsParser
    {
        public TransferBetweenAccounts ParseMessage(string body)
        {
            var match = Regex.Match(body,
                @"Debit from:\s(?<debitAccount>\*\d{4})\s+Credit to:\s(?<creditAccount>\*\d{4})\s+Amount:\s(?<amount>\d+(\.\d{1,2})?)\sSAR\s+On:\s(?<date>\d{4}-\d{1,2}-\d{1,2}\s\d{2}:\d{2})");

            if (match.Success)
            {
                return new TransferBetweenAccounts
                {
                    DebitAccount = match.Groups["debitAccount"].Value,
                    CreditAccount = match.Groups["creditAccount"].Value,
                    Amount = decimal.Parse(match.Groups["amount"].Value),
                    TransactionDate = DateTime.Parse(match.Groups["date"].Value)
                };
            }

            var lines = body.Split('\n');
            var transfer = new TransferBetweenAccounts();
            foreach (var line in lines)
            {
                if (line.Contains("Debit from:"))
                {
                    transfer.DebitAccount = line.Split(' ')[2];
                }
                if (line.Contains("Credit to:"))
                {
                    transfer.CreditAccount = line.Split(' ')[2];
                }
                if (line.Contains("Amount:"))
                {
                    transfer.Amount = decimal.Parse(line.Split(' ')[1]);
                }
                if (line.Contains("On:"))
                {
                    transfer.TransactionDate = DateTime.Parse(line.Split(' ')[1]);
                }
                if (line.Contains("Time:"))
                {
                    transfer.TransactionDate = transfer.TransactionDate.Add(TimeSpan.Parse(line.Split(' ')[1]));
                }
            }

            return transfer;
        }
    }

}
