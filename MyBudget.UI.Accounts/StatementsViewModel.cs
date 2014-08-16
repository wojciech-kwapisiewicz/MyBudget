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

namespace MyBudget.UI.Accounts
{
    public class StatementsViewModel : BindableBase
    {
        PkoBpParser _parser;
        IRepository<BankOperation> _operationRepository;
        IRepository<BankStatement> _statementsRepository;

        public StatementsViewModel(PkoBpParser parser,
            IRepository<BankOperation> operationRepository,
            IRepository<BankStatement> statementsRepository)
        {
            _parser = parser;
            _operationRepository = operationRepository;
            _statementsRepository = statementsRepository;

            LoadFileCommand = new DelegateCommand(LoadFromFile);
        }
        public ICommand LoadFileCommand { get; set; }

        public void LoadFromFile()
        {
            using (OpenFileResult file = new FileDialogService().OpenFile())
            {
                if (file.Stream == null)
                    return;

                BankStatement statement = new BankStatement()
                {
                    FileName = file.FileName
                };
                _statementsRepository.Add(statement);

                foreach (var item in OnlyNew(
                    _parser.Parse(file.Stream), 
                    _operationRepository.GetAll()))
                {
                    _operationRepository.Add(item);
                }
            }
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
    }
}
