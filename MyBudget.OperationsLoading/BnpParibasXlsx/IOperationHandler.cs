using MyBudget.Model;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.OperationsLoading.BnpParibasXlsx
{
    public interface IOperationHandler
    {
        void Handle(BankOperation operation, string description, string counterpartyInfo);
    }
}
