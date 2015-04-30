using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.PubSubEvents;
using MyBudget.UI.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.UI
{
    public class AggregationViewModel
    {
        private IEventAggregator _eventAggregator;

        public AggregationViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<ShowMessageBoxEvent>().Subscribe(RaiseNotification);

            NotificationRequest = new InteractionRequest<MessageBoxNotification>();
        }

        public InteractionRequest<MessageBoxNotification> NotificationRequest { get; set; }

        private void RaiseNotification(ShowMessageBoxEventParameter parameter)
        {
            NotificationRequest.Raise(
                new MessageBoxNotification()
                {
                    Content = parameter.Content,
                    Title = parameter.Header,
                    Buttons = parameter.Buttons,
                    Continuation = parameter.Continuation
                }, Continuation);
        }

        private void Continuation(MessageBoxNotification n)
        {
            n.Continuation(n.Result);
        }
    }
}
