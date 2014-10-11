using Microsoft.Practices.Prism.Commands;
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

namespace MyBudget.UI.Main
{
    /// <summary>
    /// Interaction logic for LanguageSetupView.xaml
    /// </summary>
    public partial class LanguageSetupView : UserControl
    {
        public LanguageSetupView()
        {

        }

        public LanguageSetupView(LanguageSetupViewModel viewModel)
        {
            RestartCommand = new DelegateCommand(Restart);
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        public ICommand RestartCommand { get; set; }
        public void Restart()
        {
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        public LanguageSetupViewModel ViewModel { get; set; }
    }
}
