using MyBudget.Core.DataContext;
using MyBudget.Core.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Core.Model
{
    public class BankOperationType : IIdentifiable<string>, IComparable   
    {
        [DontDisplayAttribute]
        public string Id
        {
            get { return Name; }
        }

        [LocalDescription("Nazwa",Language.Polish)]
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
