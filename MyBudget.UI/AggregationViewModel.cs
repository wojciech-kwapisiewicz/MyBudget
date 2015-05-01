using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.PubSubEvents;
using MyBudget.UI.Core.Popups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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
            #region This doesent work properly unfortunately 
            //CustomPopup does not scale for multiple calls. 
            //This probably is better for regular popups than for messageBox type popup.

            //todo: Remove 
            //NotificationRequest.Raise(
            //    new MessageBoxNotification()
            //    {
            //        Content = parameter.Content,
            //        Title = parameter.Caption,
            //        Buttons = parameter.Buttons,
            //        Continuation = parameter.Continuation
            //    }, Continuation);
            #endregion

            var buttonDefaultStyle = (Style)Application.Current.FindResource(typeof(Button));            
            System.Windows.Style style = new System.Windows.Style();
            style.Setters.Add(new Setter(Xceed.Wpf.Toolkit.MessageBox.YesButtonContentProperty,
                MyBudget.UI.Core.Resources.Translations.YesButton));
            style.Setters.Add(new Setter(Xceed.Wpf.Toolkit.MessageBox.NoButtonContentProperty,
                MyBudget.UI.Core.Resources.Translations.NoButton));
            style.Setters.Add(new Setter(Xceed.Wpf.Toolkit.MessageBox.OkButtonContentProperty, 
                MyBudget.UI.Core.Resources.Translations.OkButton));
            style.Setters.Add(new Setter(Xceed.Wpf.Toolkit.MessageBox.CancelButtonContentProperty,
                MyBudget.UI.Core.Resources.Translations.CancelButton));
            style.Setters.Add(new Setter(Xceed.Wpf.Toolkit.MessageBox.YesButtonStyleProperty, buttonDefaultStyle));
            style.Setters.Add(new Setter(Xceed.Wpf.Toolkit.MessageBox.NoButtonStyleProperty, buttonDefaultStyle));
            style.Setters.Add(new Setter(Xceed.Wpf.Toolkit.MessageBox.OkButtonStyleProperty, buttonDefaultStyle));
            style.Setters.Add(new Setter(Xceed.Wpf.Toolkit.MessageBox.CancelButtonStyleProperty, buttonDefaultStyle));

            MessageBoxResult defaultButton = MessageBoxResult.None;
            switch (parameter.Buttons)
            {
                case MessageBoxButton.OK:
                    defaultButton = MessageBoxResult.OK;
                    break;
                case MessageBoxButton.OKCancel:
                case MessageBoxButton.YesNoCancel:
                    defaultButton = MessageBoxResult.Cancel;
                    break;
                case MessageBoxButton.YesNo:
                    defaultButton = MessageBoxResult.No;
                    break;
                default:
                    break;
            }

            var result = Xceed.Wpf.Toolkit.MessageBox.Show(
                parameter.Content,
                parameter.Caption,
                parameter.Buttons,
                MessageBoxImage.Information,                
                defaultButton,
                style);

            if (parameter.Continuation != null)
            {
                parameter.Continuation(result);
            }
        }

        private void Continuation(MessageBoxNotification n)
        {
            if (n.Continuation != null)
            {
                n.Continuation(n.Result);
            }
        }
    }
}
