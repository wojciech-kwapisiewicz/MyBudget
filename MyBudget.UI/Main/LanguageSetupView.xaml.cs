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
        public LanguageSetupView(LanguageSetupViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        public LanguageSetupViewModel ViewModel { get; set; }
    }
}
