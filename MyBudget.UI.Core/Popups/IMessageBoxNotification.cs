using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyBudget.UI.Core.Popups
{
    public interface IMessageBoxNotification : INotification
    {
        MessageBoxButton Buttons { get; set; }

        MessageBoxResult Result { get; set; }
    }
}
