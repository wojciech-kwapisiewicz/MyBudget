using MyBudget.Core.DataContext;
using MyBudget.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyBudget.Classification
{
    public class RegularExpressionClassificationRule : IClassificationRule
    {
        public ClassificationDefinition Definition { get; private set; }
        
        private IRepository<BankAccount> _accountsRepository;

        public RegularExpressionClassificationRule(ClassificationDefinition definition, IRepository<BankAccount> accountsRepository)
        {
            Definition = definition;
            _accountsRepository = accountsRepository;
        }

        public CustomDescription GetCustomDescription()
        {
            return new CustomDescription() { Category = Definition.Category, SubCategory = Definition.SubCategory };
        }

        public bool DoMatch(BankOperation operation)
        {
            return Definition.Rules.Any(rule =>
                MatchesAccount(rule.Account, operation.BankAccount) &&
                MatchesCounterAccount(rule.CounterAccount, operation.CounterAccount) &&
                MatchesDescription(rule,operation));
        }

        private bool MatchesDescription(ClassificationRule rule, BankOperation operation)
        {
            if(string.IsNullOrEmpty( rule.RegularExpression))
            {
                return true;
            }
            return operation.Description
                .IndexOf(rule.RegularExpression, StringComparison.OrdinalIgnoreCase) >= 0;
            //return Regex
            //  .IsMatch(operation.Description, rule.RegularExpression, RegexOptions.IgnoreCase);
        }

        private bool MatchesAccount(string ruleAccountString, BankAccount operationAccount)
        {
            if (ruleAccountString == ClassificationRule.SavedAccount)
            {
                return _accountsRepository.GetAll()
                    .Any(a => a.Id == operationAccount.Id);
            }
            else if (!string.IsNullOrEmpty(ruleAccountString))
            {
                return ruleAccountString == operationAccount.Number ||
                     ruleAccountString == operationAccount.Name;
            }
            else
            {
                return true;
            }
        }

        private bool MatchesCounterAccount(string ruleAccountString, string counterAccount)
        {
            if (ruleAccountString == ClassificationRule.SavedAccount)
            {
                var accounts = _accountsRepository.GetAll().ToArray();
                var matches = accounts.Any(a =>
                    a.Number == counterAccount ||
                    (!string.IsNullOrEmpty(a.Name) && a.Name == counterAccount));
                return matches;
            }
            else if (!string.IsNullOrEmpty(ruleAccountString))
            {
                return ruleAccountString == counterAccount;
            }
            else
            {
                return true;
            }
        }
    }
}
