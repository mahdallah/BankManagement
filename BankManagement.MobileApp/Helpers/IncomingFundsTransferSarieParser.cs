using BankManagement.MobileApp.Models;
using System.Text.RegularExpressions;

namespace BankManagement.MobileApp.Helpers
{
    public class IncomingFundsTransferSarieParser
    {
        public IncomingFundsTransferSarie ParseMessage(string body)
        {
            var match = Regex.Match(body,
                @"To Account:\s(?<toAccount>\*\*\d{4})\s+Amount:\s(?<amount>\d+(\.\d{1,2})?)\sSAR\s+From:\s(?<from>.+)\s+From Account:\s(?<fromAccount>\*\*\d{4})\s+From Bank:\s(?<fromBank>.+)\s+At:\s(?<date>\d{4}-\d{1,2}-\d{1,2}\s\d{2}:\d{2})\s+Ref:\s(?<reference>.+)");

            if (match.Success)
            {
                return new IncomingFundsTransferSarie
                {
                    ToAccount = match.Groups["toAccount"].Value,
                    Amount = decimal.Parse(match.Groups["amount"].Value),
                    From = match.Groups["from"].Value,
                    FromAccount = match.Groups["fromAccount"].Value,
                    FromBank = match.Groups["fromBank"].Value,
                    TransactionDate = DateTime.Parse(match.Groups["date"].Value),
                    Reference = match.Groups["reference"].Value
                };
            }

            var lines = body.Split('\n');
            var incomingTransfer = new IncomingFundsTransferSarie();
            foreach (var line in lines)
            {
                if (line.Contains("To Account:"))
                {
                    incomingTransfer.ToAccount = line.Split(' ')[2];
                }
                if (line.Contains("Amount:"))
                {
                    incomingTransfer.Amount = decimal.Parse(line.Split(' ')[1]);
                }
                if (line.Contains("From:"))
                {
                    incomingTransfer.From = line.Split(' ')[1];
                }
                if (line.Contains("From Account:"))
                {
                    incomingTransfer.FromAccount = line.Split(' ')[2];
                }
                if (line.Contains("From Bank:"))
                {
                    incomingTransfer.FromBank = line.Split(' ')[2];
                }
                if (line.Contains("At:"))
                {
                    incomingTransfer.TransactionDate = DateTime.Parse(line.Split(' ')[1]);
                }
                if (line.Contains("Ref:"))
                {
                    incomingTransfer.Reference = line.Split(' ')[1];
                }
                if (line.Contains("Time:"))
                {
                    incomingTransfer.TransactionDate = incomingTransfer.TransactionDate.Add(TimeSpan.Parse(line.Split(' ')[1]));
                }
            }

            return incomingTransfer;
        }
    }

}
