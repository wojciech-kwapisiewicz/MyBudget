using MyBudget.Core.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Model
{
    public class ClassificationDefinition : IIdentifiable<int>
    {
        public ClassificationDefinition()
        {
            Rules = new List<ClassificationRule>();
        }

        public int Id { get; set; }
        public string Description { get; set; }

        public string Category { get; set; }
        public string SubCategory { get; set; }

        public List<ClassificationRule> Rules { get; set; }
    }
}
