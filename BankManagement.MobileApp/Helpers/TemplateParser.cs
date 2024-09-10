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
    public class TemplateParser
    {
        public object ParseSmsMessage(string body)
        {
            if (Regex.IsMatch(body, @"Online Purchase", RegexOptions.IgnoreCase))
            {
                return ParseOnlinePurchase(body);
            }
            else if (Regex.IsMatch(body, @"Purchase with mada Pay App", RegexOptions.IgnoreCase))
            {
                return ParseMadaPayPurchase(body);
            }
            else if (Regex.IsMatch(body, @"mada Atheer POS Purchase", RegexOptions.IgnoreCase))
            {
                return ParseMadaAtheerPosPurchase(body);
            }
            else if (Regex.IsMatch(body, @"Transfer Between Your Accounts", RegexOptions.IgnoreCase))
            {
                return ParseTransferBetweenAccounts(body);
            }
            else if (Regex.IsMatch(body, @"Incoming Funds Transfer \(Sarie\)", RegexOptions.IgnoreCase))
            {
                return ParseIncomingFundsTransferSarie(body);
            }
            else if (Regex.IsMatch(body, @"POS Purchase", RegexOptions.IgnoreCase))
            {
                return ParsePosPurchase(body);
            }
            else
            {
                return new UnknownTemplate { RawMessage = body };
            }
        }

        private MadaAtheerPosPurchase ParseMadaAtheerPosPurchase(string body)
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

        private TransferBetweenAccounts ParseTransferBetweenAccounts(string body)
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

        private IncomingFundsTransferSarie ParseIncomingFundsTransferSarie(string body)
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

        private PosPurchase ParsePosPurchase(string body)
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

        private MadaPayPurchase ParseMadaPayPurchase(string body)
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

        private OnlinePurchase ParseOnlinePurchase(string body)
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
