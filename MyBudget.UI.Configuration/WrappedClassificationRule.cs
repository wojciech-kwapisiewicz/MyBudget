using Microsoft.Practices.Prism.Mvvm;
using MyBudget.Core;
using MyBudget.Model;
using MyBudget.UI.Core.Selection;
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
        public WrappedClassificationRule(ClassificationRule rule, ObservableCollection<ILabel> accountDefinitions)
        {
            Data = rule;
            AccountDefinitions = accountDefinitions;
            //Add to list accounts from existing rule if the are not already present
            EnsureAccountOnList(AccountDefinitions, Data.Account);
            EnsureAccountOnList(AccountDefinitions, Data.CounterAccount);
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
                //For some reason Disabled items in ComboBox can get selected when 
                //typing by hand in IsEditable="True" mode
                //This is workaround - set new value only if value is not disabled
                if (AccountDefinitions.OfType<Splitter>()
                    .FirstOrDefault(a => a.Text == value) == null)
                {
                    SelectableItem selectedItem = GetAccountItemFromLabel(value);

                    _SelectedAccount = selectedItem.Text;
                    Data.Account = selectedItem.Value;
                }

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
                //For some reason Disabled items in ComboBox can get selected when 
                //typing by hand in IsEditable="True" mode
                //This is workaround - set new value only if value is not disabled
                if (AccountDefinitions.OfType<Splitter>()
                    .FirstOrDefault(a => a.Text == value) == null)
                {
                    SelectableItem selectedItem = GetAccountItemFromLabel(value);

                    _SelectedCounterAccount = selectedItem.Text;
                    Data.CounterAccount = selectedItem.Value;
                }

                OnPropertyChanged(() => SelectedCounterAccount);
            }
        }

        private void EnsureAccountOnList(ObservableCollection<ILabel> accountDef, string account)
        {
            if (!accountDef.OfType<SelectableItem>().Any(a => CustomCompareStrings.AreEqual(a.Value, account)))
            {
                //Insert before insertion prompt which is last
                accountDef.Insert(
                    AccountDefinitions.Count - 1, 
                    new SelectableItem() { Text = account, Value = account });
            }
        }

        private string GetAccountLabelFromDefinition(string data)
        {
            var foundDef = AccountDefinitions.OfType<SelectableItem>()
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

        private SelectableItem GetAccountItemFromLabel(string value)
        {
            //Pick by Label (selected by explicitly label/ToString() value )
            SelectableItem selectedItem =
                AccountDefinitions.OfType<SelectableItem>().FirstOrDefault(a => CustomCompareStrings.AreEqual(a.Text, value)) ??
                //If it does not exist then pick by Value (selected implicitly by account number value)
                AccountDefinitions.OfType<SelectableItem>().FirstOrDefault(a => CustomCompareStrings.AreEqual(a.Value, value));

            //If not found it means account was entered manually and is not existing
            if (selectedItem == null)
            {
                selectedItem = new SelectableItem() { Text = value, Value = value };
                //Insert before insertion prompt which is last
                AccountDefinitions.Insert(
                    AccountDefinitions.Count - 1, 
                    selectedItem);
            }

            return selectedItem;
        }
    }
}
