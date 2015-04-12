using MyBudget.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Classification
{
    public class RuleMatch
    {
        public ClassificationRule Rule { get; set; }
        public CustomDescription Description { get; set; }
    }
}
