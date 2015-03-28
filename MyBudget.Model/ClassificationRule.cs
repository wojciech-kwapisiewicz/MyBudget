using MyBudget.Core.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Model
{
    public class ClassificationRule : IIdentifiable<int>
    {
        public int Id { get; set; }
        public RuleType Type { get; set; }
        public string Description { get; set; }
        public string FieldName { get; set; }
        public string Parameter { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
    }
}
