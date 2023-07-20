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

        public IEnumerable<BankOperation> ImportOperations(string fileName, IEnumerable<BankOperation> opsToLoad)
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
                        AddNew(statement, item);
                        break;
                    case CheckStatus.Existing:
                        statement.Skipped++;
                        break;
                    case CheckStatus.ExistingUncleared:
                        UpdateExisting(statement, status);
                        break;
                }
            }

            _statementsRepository.Add(statement);

            return statement.Operations;
        }

        private void UpdateExisting(BankStatement statement, CheckResult status)
        {
            var currentStatement = _statementsRepository.GetAll()
                .First(a => a.Operations.Any(b => b.Id == status.ExistingOperation.Id));
            var opToRemove = currentStatement.Operations.First(a => a.Id == status.ExistingOperation.Id);
            currentStatement.Operations.Remove(opToRemove);
            currentStatement.Replaced++;                
            statement.Operations.Add(status.ExistingOperation);
            statement.Updated++;
            status.ExistingOperation.Cleared = true;
        }

        private void AddNew(BankStatement statement, BankOperation item)
        {
            _operationRepository.Add(item);
            statement.Operations.Add(item);
            statement.New++;
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
                    DescriptionsAreEqual(a.Description, boToCheck.Description));

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

        private string NormalizeString(string input)
        {
            return System.Text.RegularExpressions.Regex.Replace(input, @"\s", string.Empty);
        }

        private bool DescriptionsAreEqual(string description1, string description2)
        {
            if (description1 == null)
                description1 = string.Empty;
            if (description2 == null)
                description2 = string.Empty;
            var ndesc1 = NormalizeString(description1);
            var ndesc2 = NormalizeString(description2);

            return string.Equals(ndesc1, ndesc2, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
