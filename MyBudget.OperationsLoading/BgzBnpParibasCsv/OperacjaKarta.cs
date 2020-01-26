using MyBudget.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyBudget.OperationsLoading.BgzBnpParibasCsv
{
    public class OperacjaKarta : IFillOperationFromDescriptionChain
    {
        private const string Pattern = @"OPERACJA KARTĄ .* ([0-9]{6}X{6}[0-9]{4}) [0-9]{6} TRAN SAKCJA BEZGOTOWKOWA (.*) ([1-9][0-9]*.[0-9]{2}[A-Z]{3}) D=([0-9]{2}.[0-9]{2}.[0-9]{4}).*";
        private const string Type = "TRANSAKCJA KARTĄ PŁATNICZĄ";

        private IFillOperationFromDescriptionChain _next;
        private IRepositoryHelper _repositoryHelper;
        private ParseHelper _parseHelper;

        public OperacjaKarta(
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
            operation.Card = _repositoryHelper.GetOrAddCard(match.Groups[1].Value);
            operation.Description = match.Groups[2].Value.Trim();
            operation.OrderDate = _parseHelper.ParseDate(match.Groups[4].Value, "dd.MM.yyyy");

            if (operation.Description.Length > 15 && operation.Description[15] == ' ')
            {
                operation.Description = operation.Description.Remove(15, 1);
            }
        }
    }
}
