using Microsoft.Practices.Prism.Mvvm;
using MyBudget.Core;
using MyBudget.Model;
using MyBudget.UI.Core.Converters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.UI.Configuration
{
    public class WrappedClassificationRule : BindableBase
    {
        public WrappedClassificationRule(ClassificationRule rule, IEnumerable<BankAccount> existingAccounts)
        {
            Data = rule;
            AccountDefinitions = GetAccountsList(existingAccounts);
        }

        public ClassificationRule Data { get; private set; }

        public ObservableCollection<ILabel> AccountDefinitions { get; set; }

        private string _SelectedAccount;
        public string SelectedAccount
        {
            get
            {
                return GetAccountLabelFromDefinition(Data.Account);
            }
            set
            {
                SelectableText selectedItem = GetAccountItemFromLabel(value);

                _SelectedAccount = selectedItem.Text;
                Data.Account = selectedItem.Value;

                OnPropertyChanged(() => SelectedAccount);
            }
        }

        private string _SelectedCounterAccount;
        public string SelectedCounterAccount
        {
            get
            {
                return GetAccountLabelFromDefinition(Data.CounterAccount);
            }
            set
            {
                SelectableText selectedItem = GetAccountItemFromLabel(value);

                _SelectedCounterAccount = selectedItem.Text;
                Data.CounterAccount = selectedItem.Value;

                OnPropertyChanged(() => SelectedCounterAccount);
            }
        }

        private ObservableCollection<ILabel> GetAccountsList(IEnumerable<BankAccount> existingAccounts)
        {
            var accountDef = new ObservableCollection<ILabel>();
            accountDef.Add(new SelectableText() { Text = "Dowolne" });
            accountDef.Add(new SelectableText() { Text = "Dowolne zapisane", Value = ClassificationRule.SavedAccount });
            accountDef.Add(new StaticText() { Text = "----Zapisane konta własne----" });
            foreach (var account in existingAccounts)
            {
                accountDef.Add(new SelectableText() { Text = account.ToString(), Value = account.Number });
            }
            accountDef.Add(new StaticText() { Text = "----Ręcznie wprowadzone----" });
            EnsureAccountOnList(accountDef, Data.Account);
            EnsureAccountOnList(accountDef, Data.CounterAccount);
            accountDef.Add(new SelectableText() { Text = "(Podaj nr konta)", Value = Guid.NewGuid().ToString() });
            return accountDef;
        }

        private void EnsureAccountOnList(ObservableCollection<ILabel> accountDef, string account)
        {
            if (!accountDef.OfType<SelectableText>().Any(a => CustomCompareStrings.AreEqual(a.Value, account)))
            {
                accountDef.Add(new SelectableText() { Text = account, Value = account });
            }
        }

        private string GetAccountLabelFromDefinition(string data)
        {
            var foundDef = AccountDefinitions.OfType<SelectableText>()
                .Single(a => CustomCompareStrings.AreEqual(a.Value, data));
            if (foundDef != null)
            {
                return foundDef.Text;
            }
            else
            {
                return null;
            }
        }

        private SelectableText GetAccountItemFromLabel(string value)
        {
            //Pick by Label (selected by explicitly label/ToString() value )
            SelectableText selectedItem =
                AccountDefinitions.OfType<SelectableText>().FirstOrDefault(a => CustomCompareStrings.AreEqual(a.Text, value)) ??
                //If it does not exist then pick by Value (selected implicitly by account number value)
                AccountDefinitions.OfType<SelectableText>().FirstOrDefault(a => CustomCompareStrings.AreEqual(a.Value, value));

            //If not found it means account was entered manually and is not existing
            if (selectedItem == null)
            {
                selectedItem = new SelectableText()
                {
                    Text = string.Format("Ręcznie wprowadzone: {0}", value),
                    Value = value
                };
                //Insert before the last (last is for insertion prompt)
                AccountDefinitions.Insert(AccountDefinitions.Count - 1, selectedItem);
            }
            return selectedItem;
        }
    }
}
