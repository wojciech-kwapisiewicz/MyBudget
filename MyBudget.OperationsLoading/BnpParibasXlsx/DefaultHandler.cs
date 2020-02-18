using MyBudget.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.OperationsLoading.BnpParibasXlsx
{
    public class DefaultHandler : IOperationHandler
    {
        private ParseHelper _parseHelper;

        public DefaultHandler(ParseHelper parseHelper)
        {
            if (parseHelper == null)
                throw new ArgumentNullException("parseHelper");
            _parseHelper = parseHelper;
        }

        public void Handle(BankOperation operation, string description, string counterpartyInfo)
        {
            operation.Title = _parseHelper.GetFirstNCharacters(description, OperationsLoadingConsts.OperationTitleLength);
        }
    }
}
