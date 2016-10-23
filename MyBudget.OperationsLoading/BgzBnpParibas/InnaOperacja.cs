using MyBudget.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.OperationsLoading.BgzBnpParibas
{
    public class InnaOperacja : IFillOperationFromDescriptionChain
    {
        private const string Type = "INNA OPERACJA";

        private IRepositoryHelper _repositoryHelper;

        public InnaOperacja(IRepositoryHelper repositoryHelper)
        {
            if (repositoryHelper == null)
                throw new ArgumentNullException("repositoryHelper");
            _repositoryHelper = repositoryHelper;
        }

        public void Match(BankOperation operation, string description)
        {
            operation.Type = _repositoryHelper.GetOrAddOperationType(Type);
            operation.Description = description.Trim();
        }
    }
}
