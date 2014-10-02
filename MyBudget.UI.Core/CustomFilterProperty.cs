using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.UI.Core
{
    public class CustomFilterProperty<T>
    {
        public string PropertyName { get; set; }
        public Func<T, string, bool> FilteringFunction { get; set; }
    }
}
