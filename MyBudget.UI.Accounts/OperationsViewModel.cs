using Microsoft.Practices.Prism.Mvvm;
using MyBudget.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.UI.Accounts
{
    public class OperationsViewModel : BindableBase
    {
        private IEnumerable<BankAccountEntry> _Data;
        public IEnumerable<BankAccountEntry> Data
        {
            get { return _Data; }
            set
            {
                _Data = value;
                OnPropertyChanged(() => Data);
            }
        }
    }
}
