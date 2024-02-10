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
    public class GenericClassificationRule : IClassificationRule
    {
        public ClassificationDefinition Definition { get; private set; }
        
        private IRepository<BankAccount> _accountsRepository;

        public GenericClassificationRule(ClassificationDefinition definition, IRepository<BankAccount> accountsRepository)
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
            if (string.IsNullOrEmpty(rule.SearchedPhrase))
            {
                return true;
            }

            foreach (var property in operation.GetType().GetProperties()
                .Where(a => a.PropertyType == typeof(string) && rule.SearchedFieldNames.Contains(a.Name)))
            {
                string fieldValue = property.GetValue(operation, null) as string;
                if (fieldValue == null) continue;

                if (rule.IsRegularExpression && Regex.IsMatch(fieldValue, rule.SearchedPhrase, RegexOptions.IgnoreCase))
                {
                    return true;
                }
                else if (fieldValue.IndexOf(rule.SearchedPhrase, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }
            }

            if (operation != null && operation.Type != null && operation.Type.Name != null)
            {
                if (rule.IsRegularExpression && Regex.IsMatch(operation.Type.Name, rule.SearchedPhrase, RegexOptions.IgnoreCase))
                {
                    return true;
                }
                else if (operation.Type.Name.IndexOf(rule.SearchedPhrase, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }
            }

            return false;
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
