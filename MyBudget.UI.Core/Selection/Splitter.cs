using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.UI.Core.Selection
{
    public class Splitter : ISelectionMarker, ILabel
    {
        public bool IsSelectable
        {
            get { return false; }
        }

        public string Text
        {
            get;
            set;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
