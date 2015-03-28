using MyBudget.Core.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Model
{
    public class BankAccount : IIdentifiable<string>
    {
        public string Id
        {
            get { return Number; }
        }

        public string Name { get; set; }
        public string Number { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return Name ?? Number;
        }
    }
}
