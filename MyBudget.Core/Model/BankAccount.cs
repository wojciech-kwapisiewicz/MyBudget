using MyBudget.Core.DataContext;
using MyBudget.Core.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Core.Model
{
    public class BankAccount : IIdentifiable<int>, IIdentifiable<string>
    {
        [DontDisplayAttribute]
        public int Id { get; set; }
        [DontDisplayAttribute]
        string IIdentifiable<string>.Id
        {
            get { return Number; }
        }

        [LocalDescription("Nazwa",Language.Polish)]
        public string Name { get; set; }
        [LocalDescription("Numer", Language.Polish)]
        public string Number { get; set; }
        [LocalDescription("Opis", Language.Polish)]
        public string Description { get; set; }

    }
}
