using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Core.ImportData
{
    public enum CheckStatus
    {
        /// <summary>
        /// Should be added
        /// </summary>
        New,
        /// <summary>
        /// Should be ommited
        /// </summary>
        Existing,
        /// <summary>
        /// Should added only if is cleared. Old should be removed in this case.
        /// </summary>
        ExistingUncleared
    }
}
