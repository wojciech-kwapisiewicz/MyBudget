using MyBudget.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyBudget.OperationsLoading.BgzBnpParibas
{
    public class Przelew : IFillOperationFromDescriptionChain
    {
        private const string Pattern = @"PRZELEW NA RACHUNEK NUMER ([0-9]{2} [0-9]{4} [0-9]{4} [0-9]{4} [0-9]{4} [0-9]{4} [0-9]{4}) (.*)";
        private const string Type = "PRZELEW";

        private IFillOperationFromDescriptionChain _next;
        private IRepositoryHelper _repositoryHelper;

        public Przelew(
            IRepositoryHelper repositoryHelper,
            IFillOperationFromDescriptionChain next)
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
            operation.CounterAccount = match.Groups[1].Value.Trim().Replace(" ", string.Empty);
            operation.Description = match.Groups[2].Value.Trim();
        }
    }
}
