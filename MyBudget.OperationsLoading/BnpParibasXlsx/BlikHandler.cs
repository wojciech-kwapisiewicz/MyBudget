using MyBudget.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.OperationsLoading.BnpParibasXlsx
{
    public class BlikHandler : IOperationHandler
    {
        private IOperationHandler _next;
        private ParseHelper _parseHelper;
        private string _supportedOperationType = "Transakcja BLIK";

        public BlikHandler(IOperationHandler next, ParseHelper parseHelper)
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

            operation.Title = GetBlikOperationTitle(description);
            operation.CounterAccount = counterpartyInfo.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)[0];
        }

        public string GetBlikOperationTitle(string description)
        {
            var transactionDetails = description.Replace("Transakcja BLIK, ", "");
            return _parseHelper.GetFirstNCharacters(transactionDetails, OperationsLoadingConsts.OperationTitleLength);
        }
    }
}
