using MyBudget.Core.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Core.Model
{
    public class BankOperationType : IIdentifiable<string>, IComparable   
    {
        public string Id
        {
            get { return Name; }
        }

        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public int CompareTo(object other)
        {
            return this.Name.CompareTo(((BankOperationType)other).Name);
        }
    }
}
