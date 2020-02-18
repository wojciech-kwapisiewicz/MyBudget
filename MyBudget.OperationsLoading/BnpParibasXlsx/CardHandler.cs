using MyBudget.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyBudget.OperationsLoading.BnpParibasXlsx
{
    public class CardHandler : IOperationHandler
    {
        IOperationHandler _next;
        ParseHelper _parseHelper;
        string _supportedOperationType = "Transakcja kartą";

        public CardHandler(IOperationHandler next, ParseHelper parseHelper)
        {
            if (next == null)
                throw new ArgumentNullException("next");
            if (parseHelper == null)
                throw new ArgumentNullException("parseHelper");
            _next = next;
            _parseHelper = parseHelper;
        }

        public void Handle(BankOperation operation, string description, string counterpartyInfo)
        {
            if (operation.Type.Name != _supportedOperationType &&
                !operation.Type.AlternativeNames.Contains(_supportedOperationType))
            {
                _next.Handle(operation, description, counterpartyInfo);
                return;
            }

            operation.Title = GetCardOperationDetails(description);
        }

        public string GetCardOperationDetails(string description)
        {
            string regexPattern = @"^(\d*------\d*) (\w* \w*) (.*)$";
            var matchedParts = Regex.Match(description, regexPattern);

            var cardNum = matchedParts.Groups[1].Value;
            var cardHolder = matchedParts.Groups[2].Value;
            var transactionDetails = matchedParts.Groups[3].Value;

            return _parseHelper.GetFirstNCharacters(transactionDetails, 30);
        }
    }
}
