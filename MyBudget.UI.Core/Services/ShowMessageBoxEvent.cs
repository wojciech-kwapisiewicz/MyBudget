using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyBudget.UI.Core.Services
{
    public class ShowMessageBoxEvent : PubSubEvent<ShowMessageBoxEventParameter>
    {
    }
}
