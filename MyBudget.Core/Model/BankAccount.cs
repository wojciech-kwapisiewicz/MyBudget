using MyBudget.Core.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Core.Model
{
    public class BankAccount : IIdentifiable<int>, IIdentifiable<string>
    {
        public int Id { get; set; }
        string IIdentifiable<string>.Id
        {
            get { return Number; }
        }

        public string Name { get; set; }
        public string Number { get; set; }
        public string Description { get; set; }

    }
}
