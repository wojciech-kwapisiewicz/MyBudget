using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using MyBudget.Classification;
using MyBudget.Core;
using MyBudget.Core.DataContext;
using MyBudget.Model;
using MyBudget.OperationsLoading;
using MyBudget.OperationsLoading.ImportData;
using MyBudget.UI.Core.Services;
using System;
using System.Collections;
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
        private IEnumerable<IParser> _supportedParsers;
        private OperationsImporter _importer;
        private IResolveClassificationConflicts _resolveConflicts;

        private IContext _context;
        private IRepository<BankOperation> _operationRepository;
        private IRepository<BankStatement> _statementsRepository;        

        public StatementsViewModel(IContext context, IParser[] supportedParsers, IResolveClassificationConflicts resolveConflicts)
        {
            _context = context;
            _operationRepository = context.GetRepository<IRepository<BankOperation>>();
            _statementsRepository = context.GetRepository<IRepository<BankStatement>>();            

            _supportedParsers = supportedParsers.OrderBy(a => a.Name);
            _importer = new OperationsImporter(_operationRepository, _statementsRepository);
            _resolveConflicts = resolveConflicts;
            
            ApplyRules = true;
            ChosenParser = SupportedParsers.FirstOrDefault();
            ResetListData();
            
            LoadFileCommand = new DelegateCommand(LoadFromFile);
            LoadRawTextCommand = new DelegateCommand(DoLoadFromRawText, CanLoadFromRawText);
            DeleteStatementCommand = new DelegateCommand(DeleteSelected, CanDelete);
        }

        #region Parsers

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

        #endregion

        #region List data

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

        private IEnumerable _SelectedItems;
        public IEnumerable SelectedItems
        {
            get
            {
                return _SelectedItems;
            }
            set
            {
                _SelectedItems = value;
                OnPropertyChanged(() => SelectedItems);
                DeleteStatementCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion

        public bool ApplyRules { get; set; }

        public DelegateCommand LoadFileCommand { get; set; }

        public void LoadFromFile()
        {
            List<BankOperation> overallLoadedOperations = new List<BankOperation>();
            using (OpenFileResult openFileResult = new FileDialogService()
                .OpenFile(ChosenParser.SupportedFileExtensions, true))
            {
                if(openFileResult.OpenedFiles.Count()==0)
                {
                    return;
                }

                foreach (var openedFile in openFileResult.OpenedFiles)
                {
                    var loadedOperations = _importer.ImportOperations(
                        openedFile.FileName,
                        ChosenParser.Parse(openedFile.Stream));
                    overallLoadedOperations.AddRange(loadedOperations);
                }
            }

            if (ApplyRules)
            {
                var classifier = new OperationsClassifier(
                    _context.GetRepository<IRepository<ClassificationDefinition>>(),
                    _context.GetRepository<IRepository<BankAccount>>());
                var classificationResult = classifier.ClasifyOpearations(overallLoadedOperations);
                
                _resolveConflicts.ResolveConflicts(classificationResult);
                
                var assigned = classifier.ApplyClassificationResult(classificationResult);
                var unassigned = classificationResult.Count() - assigned;

                MessageBox.Show(string.Format(
                    "Dla {0} operacji przypisano kategorię.{1}Dla {2} operacji nie przypisano kategorii.",
                    assigned, Environment.NewLine, unassigned));
            }

            _context.SaveChanges();

            ResetListData();
        }

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

        public DelegateCommand LoadRawTextCommand { get; set; }

        public bool CanLoadFromRawText()
        {
            return !string.IsNullOrWhiteSpace(RawStatementText);
        }

        public void DoLoadFromRawText()
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

        public bool CanDelete()
        {
            return SelectedItems != null && SelectedItems.OfType<BankStatement>().Count() > 0;
        }        

        public void DoDelete()
        {
            foreach (var selectedStatement in SelectedItems.OfType<BankStatement>())
            {
                foreach (var operation in selectedStatement.Operations)
                {
                    _operationRepository.Delete(operation);
                }
                _statementsRepository.Delete(selectedStatement);
            }

            _context.SaveChanges();
            ResetListData();
            OnPropertyChanged(() => Data);
        }
    }
}
