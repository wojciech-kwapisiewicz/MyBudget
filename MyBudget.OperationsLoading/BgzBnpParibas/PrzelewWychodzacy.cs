using MyBudget.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyBudget.OperationsLoading.BgzBnpParibas
{
    public class PrzelewWychodzacy : IFillOperationFromDescriptionChain
    {
        private const string Pattern = @"PRZELEW OBCIĄŻENIOWY (.*) ([0-9]{2} [0-9]{4} [0-9]{4} [0-9]{4} [0-9]{4} [0-9]{4} [0-9]{4}) (.*)";        
        private const string Type = "PRZELEW DO INNEGO BANKU";

        private IFillOperationFromDescriptionChain _next;
        private IRepositoryHelper _repositoryHelper;

        public PrzelewWychodzacy(
            IFillOperationFromDescriptionChain next,
            IRepositoryHelper repositoryHelper)
        {
            if (next == null)
                throw new ArgumentNullException("next");
            if (repositoryHelper == null)
                throw new ArgumentNullException("repositoryHelper");
            _next = next;
            _repositoryHelper = repositoryHelper;
        }

        public void Match(BankOperation operation, string description)
        {
            var match = Regex.Match(description, Pattern);
            if (!match.Success)
            {
                _next.Match(operation, description);
                return;
            }

            operation.Type = _repositoryHelper.GetOrAddOperationType(Type);
            operation.Description = match.Groups[1].Value.Trim();
            operation.CounterAccount = match.Groups[2].Value.Trim().Replace(" ", string.Empty);
        }
    }
}
