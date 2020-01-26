using MyBudget.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyBudget.OperationsLoading.BgzBnpParibasCsv
{
    public class PrzelewPrzychodzacy : IFillOperationFromDescriptionChain
    {
        private const string Pattern = @"PRZELEW UZNANIOWY \(NADANO ([0-9]{2}-[0-9]{2}-[0-9]{4})\) (.*)";
        private const string AccountNumberPattern = @"[0-9]{2} [0-9]{4} [0-9]{4} [0-9]{4} [0-9]{4} [0-9]{4} [0-9]{4}";
        private const string Type = "PRZELEW PRZYCHODZĄCY";

        private IFillOperationFromDescriptionChain _next;
        private IRepositoryHelper _repositoryHelper;
        private ParseHelper _parseHelper;

        public PrzelewPrzychodzacy(
            IRepositoryHelper repositoryHelper,
            ParseHelper parseHelper,
            IFillOperationFromDescriptionChain next)
        {
            if (next == null)
                throw new ArgumentNullException("next");
            if (repositoryHelper == null)
                throw new ArgumentNullException("repositoryHelper");
            if (parseHelper == null)
                throw new ArgumentNullException("parseHelper");
            _next = next;
            _repositoryHelper = repositoryHelper;
            _parseHelper = parseHelper;
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
            operation.OrderDate = _parseHelper.ParseDate(match.Groups[1].Value, "dd-MM-yyyy");
            operation.Description = match.Groups[2].Value.Trim();

            var accountMatch = Regex.Match(operation.Description, AccountNumberPattern);
            if(accountMatch.Success)
            {
                operation.CounterAccount = accountMatch.Value.Trim().Replace(" ", string.Empty);
            }            
        }
    }
}
