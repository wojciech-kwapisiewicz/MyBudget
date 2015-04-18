using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.UI.Core.Selection
{
    public interface ISelectionMarker
    {
        bool IsSelectable { get; }
    }
}
