using Microsoft.Practices.Prism.Commands;
using MyBudget.Core;
using MyBudget.Core.DataContext;
using MyBudget.Core.Model;
using MyBudget.UI.Core.Services;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace MyBudget.UI.Accounts
{
    /// <summary>
    /// Interaction logic for StatementsView.xaml
    /// </summary>
    public partial class StatementsView : UserControl
    {
        PkoBpParser _parser;
        IRepository<BankOperation> _operationRepository;

        public StatementsView(PkoBpParser parser,
            IRepository<BankOperation> operationRepository)
        {
            _parser = parser;
            _operationRepository = operationRepository;

            LoadFileCommand = new DelegateCommand(LoadFromFile);
            DataContext = this;
            InitializeComponent();
        }

        public ICommand LoadFileCommand { get; set; }

        public void LoadFromFile()
        {
            using (Stream stream = new FileDialogService().OpenFile())
            {
                foreach (var item in _parser.Parse(stream))
                {
                    _operationRepository.Add(item);
                }
            }
        }
    }
}
