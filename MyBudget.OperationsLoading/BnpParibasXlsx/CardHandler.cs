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
        private IOperationHandler _next;
        private ParseHelper _parseHelper;
        private IRepositoryHelper _repositoryHelper;
        private string _supportedOperationType = "Transakcja kartą";

        public CardHandler(ParseHelper parseHelper, IRepositoryHelper repositoryHelper, IOperationHandler next)
        {
            if (next == null)
                throw new ArgumentNullException("next");
            if (parseHelper == null)
                throw new ArgumentNullException("parseHelper");
            _next = next;
            _parseHelper = parseHelper;
            _repositoryHelper = repositoryHelper;
        }

        public void Handle(BankOperation operation, string description, string counterpartyInfo)
        {
            if (operation.Type.Name != _supportedOperationType &&
                !operation.Type.AlternativeNames.Contains(_supportedOperationType))
            {
                _next.Handle(operation, description, counterpartyInfo);
                return;
            }

            string regexPattern = @"^(\d*------\d*) (\w* \w*) (.*)$";
            var matchedParts = Regex.Match(description, regexPattern);

            var cardNum = matchedParts.Groups[1].Value;
            var cardHolder = matchedParts.Groups[2].Value;
            var transactionDetails = matchedParts.Groups[3].Value;


            operation.Title = _parseHelper.GetFirstNCharacters(transactionDetails, OperationsLoadingConsts.OperationTitleLength);
            operation.Card = _repositoryHelper.GetOrAddCard(cardNum);
        }
    }
}
