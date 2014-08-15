using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
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
using System.Windows.Input;
using Xceed.Wpf.Toolkit;

namespace MyBudget.UI.Accounts
{
    public class OperationsViewModel : BindableBase
    {
        PkoBpParser _parser;
        IRepository<BankOperation> _operationRepository;

        public OperationsViewModel(
            PkoBpParser parser,
            IRepository<BankOperation> operationRepository)
        {
            _operationRepository = operationRepository;
            _parser = parser;

            LoadFileCommand = new DelegateCommand(LoadFromFile);
            SaveCommand = new DelegateCommand(() => Save());
        }

        public void LoadFromFile()
        {
            using (Stream stream = new FileDialogService().OpenFile())
            {
                foreach (var item in _parser.Parse(stream))
                {
                    _operationRepository.Add(item);
                }
            }
            OnPropertyChanged(() => Data);
        }

        public IEnumerable<BankOperation> Data
        {
            get { return _operationRepository.GetAll(); }
        }

        public void Save()
        {
            MessageBox.Show("Saved!");
        }

        public ICommand LoadFileCommand { get; set; }

        public ICommand SaveCommand { get; set; }
    }
}
