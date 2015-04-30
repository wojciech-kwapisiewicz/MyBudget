using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Practices.Prism.Interactivity;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using MyBudget.UI.Core.Popups;
using Microsoft.Practices.Prism.PubSubEvents;

namespace MyBudget.UI.Main
{
    public class WelcomPageViewModel : BindableBase
    {
        private string InnerText = "Inner text";
        private string InnerCaption = "Inner caption";

        private IEventAggregator _eventAggregator;
        private IMessageBoxService _messageBoxService;

        public WelcomPageViewModel(IEventAggregator eventAggregator, IMessageBoxService messageBoxService)
        {
            _eventAggregator = eventAggregator;
            _messageBoxService = messageBoxService;
            ShowStandardMsgBoxCommand = new DelegateCommand(() => MessageBox.Show(
                InnerText, 
                InnerCaption,
                MessageBoxButton.YesNo,
                MessageBoxImage.Information,
                MessageBoxResult.No));
            ShowToolkitMsgBoxCommand = new DelegateCommand(ShowToolkit);
            ShowViaEventAggregatorCommand = new DelegateCommand(ShowViaEventAggregator);
            ShowViaMessageBoxServiceCommand = new DelegateCommand(ShowViaMessageBoxService);
        }

        private object _Status;
        public object Status
        {
            get
            {
                return _Status;
            }
            set
            {
                _Status = value;
                OnPropertyChanged(() => Status);
            }
        }

        private object _Selected;
        public object Selected
        {
            get
            {
                return _Selected;
            }
            set
            {
                _Selected = value;
            }
        }

        public ICommand ShowStandardMsgBoxCommand { get; set; }

        public ICommand ShowToolkitMsgBoxCommand { get; set; }

        void ShowToolkit()
        {
            System.Windows.Style style = new System.Windows.Style();
            style.Setters.Add(new Setter(Xceed.Wpf.Toolkit.MessageBox.YesButtonContentProperty, "Yes, FTW!"));
            style.Setters.Add(new Setter(Xceed.Wpf.Toolkit.MessageBox.NoButtonContentProperty, "Omg, no"));

            Xceed.Wpf.Toolkit.MessageBox.Show(
                InnerText,
                InnerCaption,
                MessageBoxButton.YesNo,
                MessageBoxImage.Information,
                MessageBoxResult.No,
                style);
        }

        public ICommand ShowViaEventAggregatorCommand { get; set; }

        private void ShowViaEventAggregator()
        {
            ShowMessageBoxEventParameter parameter = new ShowMessageBoxEventParameter()
            {
                Buttons = MessageBoxButton.OKCancel,
                Caption = "???",
                Content = "!!!!"
            };
            _eventAggregator.GetEvent<ShowMessageBoxEvent>().Publish(parameter);
        }

        public ICommand ShowViaMessageBoxServiceCommand { get; set; }

        private void ShowViaMessageBoxService()
        {
            _messageBoxService.ShowMessageBox(
                "Sent via service", 
                "Conent was sent via service. How cool is that? :)",
                MessageBoxButton.OKCancel, 
                null);
        }
    }


    /// <summary>
    /// Interaction logic for WelcomePageView.xaml
    /// </summary>
    public partial class WelcomePageView : UserControl, IRegionMemberLifetime
    {
        public WelcomePageView(WelcomPageViewModel view)
        {
            this.DataContext = view;
            InitializeComponent();
        }

        public bool KeepAlive
        {
            get { return false; }
        }            
    }
}
