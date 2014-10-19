using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.UI.Operations
{
    public class Splitter : IGroupItem
    {
        public string Key { get; set; }
        public string Sum { get; set; }
    }
}
