using MyBudget.Core.DataContext;
using MyBudget.Core.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Core.Model
{
    public class BankOperationType : IIdentifiable<string>
    {
        [DontDisplayAttribute]
        public string Id
        {
            get { return Name; }
        }

        [LocalDescription("Nazwa",Language.Polish)]
        public string Name { get; set; }
    }
}
