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
        //public object ParseSmsMessage(string body)
        //{
        //    if (Regex.IsMatch(body, @"Online Purchase", RegexOptions.IgnoreCase))
        //    {
        //        return ParseOnlinePurchase(body);
        //    }
        //    else if (Regex.IsMatch(body, @"Purchase with mada Pay App", RegexOptions.IgnoreCase))
        //    {
        //        return ParseMadaPayPurchase(body);
        //    }
        //    else if (Regex.IsMatch(body, @"mada Atheer POS Purchase", RegexOptions.IgnoreCase))
        //    {
        //        return ParseMadaAtheerPosPurchase(body);
        //    }
        //    else if (Regex.IsMatch(body, @"Transfer Between Your Accounts", RegexOptions.IgnoreCase))
        //    {
        //        return ParseTransferBetweenAccounts(body);
        //    }
        //    else if (Regex.IsMatch(body, @"Incoming Funds Transfer \(Sarie\)", RegexOptions.IgnoreCase))
        //    {
        //        return ParseIncomingFundsTransferSarie(body);
        //    }
        //    else if (Regex.IsMatch(body, @"POS Purchase", RegexOptions.IgnoreCase))
        //    {
        //        return ParsePosPurchase(body);
        //    }
        //    else
        //    {
        //        return new UnknownTemplate { RawMessage = body };
        //    }
        //}
    }
}
