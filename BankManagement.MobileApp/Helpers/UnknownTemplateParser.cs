using BankManagement.MobileApp.Models;

namespace BankManagement.MobileApp.Helpers
{
    public class UnknownTemplateParser
    {
        public UnknownTemplate ParseMessage(string body)
        {
            return new UnknownTemplate { RawMessage = body };
        }
    }

}
