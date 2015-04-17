using Microsoft.Practices.Prism.Mvvm;
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

    public static class StringComparison
    {
        public static bool AreEqual(string a, string b)
        {
            return (a ?? string.Empty) == (b ?? string.Empty);
        }
    }

    public class WrappedClassificationRule : BindableBase
    {
        public WrappedClassificationRule(ClassificationRule rule, IEnumerable<BankAccount> existingAccounts)
        {
            Data = rule;
            AccountDefinitions = GetAccountsList(existingAccounts);
        }

        public readonly string AnyLabel = "Dowolne";

        private ObservableCollection<ILabel> GetAccountsList(IEnumerable<BankAccount> existingAccounts)
        {
            var accountDef = new ObservableCollection<ILabel>();
            accountDef.Add(new SelectableText() { Text = AnyLabel });
            accountDef.Add(new SelectableText() { Text = "Dowolne zapisane", Value = ClassificationRule.SavedAccount });
            accountDef.Add(new StaticText() { Text = "----Zapisane konta własne----" });
            foreach (var account in existingAccounts)
            {
                accountDef.Add(new SelectableText() { Text = account.ToString(), Value = account.Number });
            }            
            accountDef.Add(new StaticText() { Text = "----Ręcznie wprowadzone----" });
            if (!accountDef.OfType<SelectableText>().Any(a => StringComparison.AreEqual(a.Value, Data.Account)))
            {
                accountDef.Add(new SelectableText() { Text = Data.Account, Value = Data.Account });
            }
            accountDef.Add(new SelectableText() { Text = "(Podaj nr konta)", Value = Guid.NewGuid().ToString() });
            return accountDef;
        }

        public ClassificationRule Data { get; private set; }

        public ObservableCollection<ILabel> AccountDefinitions { get; set; }

        private SelectableText _SelectedItem;

        private string _SelectedAccount;
        public string SelectedAccount
        {
            get
            {
                var foundDef = AccountDefinitions.OfType<SelectableText>()
                    .Single(a => StringComparison.AreEqual(a.Value, Data.Account));
                if (foundDef != null)
                {
                    return foundDef.Text;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                //Pick by Label (selected by explicitly label/ToString() value )
                SelectableText selectedItem =
                    AccountDefinitions.OfType<SelectableText>().FirstOrDefault(a => StringComparison.AreEqual(a.Text, value)) ??
                    //If it does not exist then pick by Value (selected implicitly by account number value)
                    AccountDefinitions.OfType<SelectableText>().FirstOrDefault(a => StringComparison.AreEqual(a.Value, value));

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

                _SelectedItem = selectedItem;
                _SelectedAccount = selectedItem.Text;
                Data.Account = selectedItem.Value;

                OnPropertyChanged(() => SelectedAccount);
            }
        }
    }
}
