using MyBudget.Core.DataContext;
using MyBudget.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.OperationsLoading
{
    public class RepositoryHelper : IRepositoryHelper
    {
        private IRepository<BankAccount, string> _bankAccountRepository;
        private IRepository<BankOperationType, string> _bankOperationTypeRepository;

        public RepositoryHelper(
            IRepository<BankAccount, string> bankAccountRepository,
            IRepository<BankOperationType, string> bankOperationTypeRepository)
        {
            if (bankAccountRepository == null)
                throw new ArgumentNullException("bankAccountRepository");
            if (bankOperationTypeRepository == null)
                throw new ArgumentNullException("bankOperationTypeRepository");
            _bankAccountRepository = bankAccountRepository;
            _bankOperationTypeRepository = bankOperationTypeRepository;
        }

        public BankAccount GetOrAddAccount(string accountNumber)
        {
            //accountNumber = MaskAccountNumber(accountNumber);

            BankAccount account = _bankAccountRepository.Get(accountNumber);
            if (account == null)
            {
                account = new BankAccount();
                account.Number = accountNumber;
                _bankAccountRepository.Add(account);
            }

            return account;
        }

        public BankOperationType GetOrAddOperationType(string typeName)
        {
            BankOperationType operationType = _bankOperationTypeRepository
                .GetAll().Where(a => a.Name == typeName || a.AlternativeNames.Contains(typeName))
                .SingleOrDefault();
            if (operationType == null)
            {
                operationType = new BankOperationType();
                operationType.Name = typeName;
                operationType.AlternativeNames = typeName;
                _bankOperationTypeRepository.Add(operationType);
            }
            return operationType;
        }

        //private static string MaskAccountNumber(string accountNumber)
        //{
        //    int frontCiphersCount = 6;
        //    int backCiphersCount = 4;

        //    string front = accountNumber.Substring(0, frontCiphersCount);
        //    string back = accountNumber.Substring(accountNumber.Length - backCiphersCount, backCiphersCount);
        //    string mask = new string('*', accountNumber.Length - frontCiphersCount - backCiphersCount);

        //    return string.Format("{0}{1}{2}", front, mask, back);
        //}
    }
}
