using MyBudget.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.OperationsLoading.BnpParibasXlsx
{
    public class BankTransferHandler : IOperationHandler
    {
        private IOperationHandler _next;
        private ParseHelper _parseHelper;

        public string[] supportedOperations = new string[] { "Przelew przychodzący", "Przelew wychodzący", "Przelew internetowy", "Zlecenie stałe" };

        public BankTransferHandler(ParseHelper parseHelper, IOperationHandler next)
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
            if (!supportedOperations.Contains(operation.Type.Name))
            {
                _next.Handle(operation, description, counterpartyInfo);
                return;
            }

            operation.Title = _parseHelper.GetFirstNCharacters(description, OperationsLoadingConsts.OperationTitleLength);
            operation.CounterAccount = counterpartyInfo.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)[0];
        }
    }
}
