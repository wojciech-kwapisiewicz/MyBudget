using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyBudget.UI.Core.Popups
{
    public class MessageBoxService : IMessageBoxService
    {
        IEventAggregator _eventAggregator;

        public MessageBoxService(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public void ShowMessageBox(string caption, string conent, MessageBoxButton buttons, Action<MessageBoxResult> continuation)
        {
            _eventAggregator
                .GetEvent<ShowMessageBoxEvent>()
                .Publish(new ShowMessageBoxEventParameter()
                {
                    Caption = caption,
                    Content = conent,
                    Buttons = buttons,
                    Continuation = continuation
                });
        }
    }
}
