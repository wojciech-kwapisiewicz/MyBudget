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
        public const string SavedAccount = "SavedAccount";
        [System.Xml.Serialization.XmlIgnore]
        public readonly List<string> SearchedFieldNames = new List<string>() { "Description", "Title", "CounterParty" };
        public const RuleType Type = RuleType.Mixed;
        
        public int Id { get; set; }

        public string Account { get; set; }
        public string CounterAccount { get; set; }
        public string SearchedPhrase { get; set; }
        public bool IsRegularExpression { get; set; }
    }
}
