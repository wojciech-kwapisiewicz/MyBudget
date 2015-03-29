using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using MyBudget.Core;
using MyBudget.Core.DataContext;
using MyBudget.Model;
using MyBudget.OperationsLoading;
using MyBudget.OperationsLoading.ImportData;
using MyBudget.UI.Core.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace MyBudget.UI.Accounts
{
    public class StatementsViewModel : BindableBase
    {
        IEnumerable<IParser> _supportedParsers;
        IRepository<BankOperation> _operationRepository;
        IRepository<BankStatement> _statementsRepository;
        OperationsImporter _importer;
        IContext _context;

        public StatementsViewModel(IContext context, IParser[] supportedParsers)
        {
            _context = context;
            _supportedParsers = supportedParsers.OrderBy(a => a.Name);
            ChosenParser = SupportedParsers.FirstOrDefault();
            _operationRepository = context.GetRepository<IRepository<BankOperation>>();
            _statementsRepository = context.GetRepository<IRepository<BankStatement>>();
            _importer = new OperationsImporter(_operationRepository, _statementsRepository);
            ResetListData();
            LoadFileCommand = new DelegateCommand(LoadFromFile);
            LoadRawTextCommand = new DelegateCommand(LoadFromRawText, CanLoadFromRawText);
            DeleteStatementCommand = new DelegateCommand(DeleteSelected, () => Selected != null);
        }

        public IEnumerable<IParser> SupportedParsers
        {
            get
            {
                return _supportedParsers;
            }
        }

        private IParser _supportedParser;
        public IParser ChosenParser
        {
            get
            {
                return _supportedParser;
            }
            set
            {
                _supportedParser = value;
                OnPropertyChanged(() => SupportedParsers);
            }
        }

        private void ResetListData()
        {
            var list = new ListCollectionView(_statementsRepository.GetAll().ToList());
            Data = list;
        }

        private ListCollectionView _data;
        public ListCollectionView Data
        {
            get
            {

                return _data;
            }
            set
            {
                _data = value;
                OnPropertyChanged(() => Data);
            }
        }

        private BankStatement _selected;
        public BankStatement Selected
        {
            get
            {
                return _selected;
            }
            set
            {
                _selected = value;
                OnPropertyChanged(() => Selected);
                DeleteStatementCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand LoadFileCommand { get; set; }

        public void LoadFromFile()
        {
            using (OpenFileResult file = new FileDialogService().OpenFile(ChosenParser.SupportedFileExtensions))
            {
                if (file.Stream == null)
                    return;

                _importer.ImportOperations(file.FileName, ChosenParser.Parse(file.Stream));
                
                _context.SaveChanges();
            }

            ResetListData();
        }

        public DelegateCommand LoadRawTextCommand { get; set; }

        private string _rawStatementText;
        public string RawStatementText
        {
            get
            {
                return _rawStatementText;
            }
            set
            {
                _rawStatementText = value;
                OnPropertyChanged(() => RawStatementText);
                LoadRawTextCommand.RaiseCanExecuteChanged();
            }
        }

        public bool CanLoadFromRawText()
        {
            return !string.IsNullOrWhiteSpace(RawStatementText);
        }

        public void LoadFromRawText()
        {                        
            string statementName = "FromText" + DateTime.Now;
            _importer.ImportOperations(statementName, ChosenParser.Parse(RawStatementText));
            _context.SaveChanges();
            ResetListData();
        }

        public DelegateCommand DeleteStatementCommand { get; set; }

        public void DeleteSelected()
        {
            var result = MessageBox.Show(
                Resources.Translations.DeleteStatementWarningText,
                Resources.Translations.DeleteStatementWarningCaption,
                System.Windows.MessageBoxButton.YesNo);
            if (result == System.Windows.MessageBoxResult.Yes)
            {
                DoDelete();
            }
        }

        public void DoDelete()
        {
            foreach (var item in Selected.Operations)
            {
                _operationRepository.Delete(item);
            }
            _statementsRepository.Delete(Selected);
            _context.SaveChanges();
            ResetListData();
            OnPropertyChanged(() => Data);
        }
        
    }
}
