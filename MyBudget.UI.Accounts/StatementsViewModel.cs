using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using MyBudget.Core;
using MyBudget.Core.DataContext;
using MyBudget.Core.ImportData;
using MyBudget.Core.Model;
using MyBudget.UI.Core.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using Xceed.Wpf.Toolkit;

namespace MyBudget.UI.Accounts
{
    public class StatementsViewModel : BindableBase
    {
        IParser[] _supportedParsers;
        IRepository<BankOperation> _operationRepository;
        IRepository<BankStatement> _statementsRepository;
        IContext _context;

        public StatementsViewModel(IContext context, IParser[] supportedParsers)
        {
            _context = context;
            _supportedParsers = supportedParsers;
            ChosenParser = SupportedParsers.FirstOrDefault();
            _operationRepository = context.GetRepository<IRepository<BankOperation>>();
            _statementsRepository = context.GetRepository<IRepository<BankStatement>>();
            ResetListData();
            LoadFileCommand = new DelegateCommand(LoadFromFile);
            LoadRawTextCommand = new DelegateCommand(LoadFromRawText, CanLoadFromRawText);
        }

        public IParser[] SupportedParsers
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

        public DelegateCommand LoadFileCommand { get; set; }

        public void LoadFromFile()
        {
            using (OpenFileResult file = new FileDialogService().OpenFile())
            {
                if (file.Stream == null)
                    return;

                BankStatement statement = new BankStatement()
                {
                    FileName = file.FileName,
                    LoadTime = DateTime.UtcNow,
                    Operations = new List<BankOperation>(),
                };

                _statementsRepository.Add(statement);

                foreach (var item in OnlyNew(
                    ChosenParser.Parse(file.Stream), 
                    _operationRepository.GetAll()))
                {
                    statement.Operations.Add(item);
                    _operationRepository.Add(item);
                }
                _context.SaveChanges();
            }

            ResetListData();
        }

        public IEnumerable<BankOperation> OnlyNew(
            IEnumerable<BankOperation> toAdd, 
            IEnumerable<BankOperation> existing)
        {
            foreach (var item in toAdd)
            {
                var alreadyExist = existing.Any(a =>
                    a.OrderDate == item.OrderDate &&
                    a.ExecutionDate == item.ExecutionDate &&
                    a.Amount == item.Amount &&
                    a.EndingBalance == item.EndingBalance &&
                    a.Description == item.Description);
                if (!alreadyExist)
                {
                    yield return item;
                }
            }
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
            var now = DateTime.UtcNow;
            BankStatement statement = new BankStatement()
            {
                FileName = "FromText" + now,
                LoadTime = now,
                Operations = new List<BankOperation>(),
            };

            _statementsRepository.Add(statement);

            foreach (var item in OnlyNew(
                ChosenParser.Parse(RawStatementText),
                _operationRepository.GetAll()))
            {
                statement.Operations.Add(item);
                _operationRepository.Add(item);
            }
            _context.SaveChanges();

            ResetListData();
        }
    }
}
