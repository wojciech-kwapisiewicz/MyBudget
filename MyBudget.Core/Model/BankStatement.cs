using MyBudget.Core.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Core.Model
{
    public class BankStatement : IIdentifiable<int>
    {
        public int Id { get; set; }

        public string FileName { get; set; }
        public DateTime LoadTime { get; set; }
        
        public List<BankOperation> Operations { get; set; }
    }
}
