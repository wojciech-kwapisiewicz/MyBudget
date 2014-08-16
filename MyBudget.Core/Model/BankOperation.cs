using MyBudget.Core.DataContext;
using MyBudget.Core.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Core.Model
{
    public class BankOperation : IIdentifiable<int>
    {
        [DontDisplay]
        public int Id { get; set; }
        [DontDisplay]
        public BankAccount BankAccount { get; set; }

        [LocalDescription("Numer na wyciągu", Language.Polish)]
        public int LpOnStatement { get; set; }
        
        [LocalDescription("Typ operacji", Language.Polish)]
        public BankOperationType Type { get; set; }
        [LocalDescription("Data zlecenia", Language.Polish)]
        public DateTime OrderDate { get; set; }
        [LocalDescription("Data wykonania", Language.Polish)]
        public DateTime ExecutionDate { get; set; }
        [LocalDescription("Kwota", Language.Polish)]
        public decimal Amount { get; set; }
        [LocalDescription("Saldo po operacji", Language.Polish)]
        public decimal EndingBalance { get; set; }
        [LocalDescription("Tytułem", Language.Polish)]
        public string Title { get; set; }
        [DontDisplay]
        [LocalDescription("Opis", Language.Polish)]
        public string Description { get; set; }
        
        [DontDisplay]
        public CustomDescription CustomDescription { get; set; }
    }
}
