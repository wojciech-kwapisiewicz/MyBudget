using MyBudget.Core.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Model
{
    public class BankStatement : IIdentifiable<int>
    {
        public BankStatement()
        {
            Operations = new List<BankOperation>();
        }
        public int Id { get; set; }

        public string FileName { get; set; }
        public DateTime LoadTime { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public List<BankOperation> Operations { get; set; }

        public int New { get; set; }
        public int Skipped { get; set; }
        public int Updated { get; set; }
        public int Replaced { get; set; }
    }
}
