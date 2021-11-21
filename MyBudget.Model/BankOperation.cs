using MyBudget.Core.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Model
{
    public class BankOperation : NotifiableObject, IIdentifiable<int>
    {
        public int Id { get; set; }
        public BankAccount BankAccount { get; set; }
        public bool Cleared { get; set; }

        public BankOperationType Type { get; set; }
        public int LpOnStatement { get; set; }        
        public DateTime OrderDate { get; set; }
        public DateTime ExecutionDate { get; set; }
        public decimal Amount { get; set; }
        public decimal EndingBalance { get; set; }
        public string Description { get; set; }

        private string _category;
        public string Category
        {
            get
            {
                return _category;
            }
            set
            {
                _category = value;
                OnPropertyChanged(() => Category);
            }
        }
        
        private string _subcategory;
        public string SubCategory
        {
            get
            {
                return _subcategory;
            }
            set
            {
                _subcategory = value;
                OnPropertyChanged(() => SubCategory);
            }
        }

        public Card Card { get; set; }

        public string Title { get; set; }
        public string CounterAccount { get; set; }

        public string CounterParty { get; set; }
    }
}
