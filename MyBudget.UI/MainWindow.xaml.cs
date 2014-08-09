using MyBudget.Core;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace MyBudget.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            string str = ManifestStreamReaderHelper.ReadEmbeddedResource(
                this.GetType().Assembly,
                "MyBudget.UI.PkoBp1Entry.xml");
            Data = new PkoBpParser().Parse(str);
            InitializeComponent();
        }

        public IEnumerable<BankAccountEntry> Data
        {
            get;
            private set;
        }

    }
}
