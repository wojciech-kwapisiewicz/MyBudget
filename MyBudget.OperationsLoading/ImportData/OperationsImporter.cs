using MyBudget.Core.DataContext;
using MyBudget.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.OperationsLoading.ImportData
{
    public class OperationsImporter
    {
        IRepository<BankOperation> _operationRepository;
        IRepository<BankStatement> _statementsRepository;

        public OperationsImporter(
            IRepository<BankOperation> operationRepository,
            IRepository<BankStatement> statementsRepository)
        {
            _operationRepository = operationRepository;
            _statementsRepository = statementsRepository;
        }

        public void ImportOperations(string fileName, IEnumerable<BankOperation> opsToLoad)
        {
            BankStatement statement = new BankStatement()
            {
                FileName = fileName,
                LoadTime = DateTime.UtcNow,                
            };

            var allOps = _operationRepository.GetAll();
            foreach (var item in opsToLoad)
            {
                var status = GetStatus(item, allOps);
                switch (status.Status)
                {
                    case CheckStatus.New:
                        _operationRepository.Add(item);
                        statement.Operations.Add(item);
                        statement.New++;
                        break;
                    case CheckStatus.Existing:
                        statement.Skipped++;
                        break;
                    case CheckStatus.ExistingUncleared:
                        status.ExistingOperation.Cleared = true;
                        //Should be removed from existing statement??
                        statement.Operations.Add(status.ExistingOperation);
                        statement.Updated++;
                        break;
                    default:
                        break;
                }
            }

            _statementsRepository.Add(statement);
        }

        public CheckResult GetStatus(
            BankOperation boToCheck,
            IEnumerable<BankOperation> existing)
        {
            BankOperation existingOperation = existing.FirstOrDefault(a =>
                    a.OrderDate == boToCheck.OrderDate &&
                    a.ExecutionDate == boToCheck.ExecutionDate &&
                    a.Amount == boToCheck.Amount &&
                    a.EndingBalance == boToCheck.EndingBalance &&
                    a.Description == boToCheck.Description);

            if (existingOperation == null)
            {
                return new CheckResult() { Status = CheckStatus.New };
            }
            if (existingOperation.Cleared == false && boToCheck.Cleared == true)
            {
                return new CheckResult() { Status = CheckStatus.ExistingUncleared, ExistingOperation = existingOperation };
            }
            return new CheckResult() { Status = CheckStatus.Existing, ExistingOperation = existingOperation };
        }
    }
}
