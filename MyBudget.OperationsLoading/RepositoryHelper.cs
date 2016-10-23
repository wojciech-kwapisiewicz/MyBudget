using MyBudget.Core.DataContext;
using MyBudget.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace MyBudget.OperationsLoading
{
    public class RepositoryHelper : IRepositoryHelper
    {
        private IRepository<BankAccount, string> _bankAccountRepository;
        private IRepository<BankOperationType, string> _bankOperationTypeRepository;
        private IRepository<Card, string> _cardRepository;

        public RepositoryHelper(
            IRepository<BankAccount, string> bankAccountRepository,
            IRepository<BankOperationType, string> bankOperationTypeRepository,
            IRepository<Card, string> cardRepository)
        {
            if (bankAccountRepository == null)
                throw new ArgumentNullException("bankAccountRepository");
            if (bankOperationTypeRepository == null)
                throw new ArgumentNullException("bankOperationTypeRepository");
            if (cardRepository == null)
                throw new ArgumentNullException("cardRepository");
            _bankAccountRepository = bankAccountRepository;
            _bankOperationTypeRepository = bankOperationTypeRepository;
            _cardRepository = cardRepository;
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

        public Card GetOrAddCard(string cardNumber)
        {
            Card card = _cardRepository.Get(cardNumber);
            if (card == null)
            {
                card = new Card();
                card.CardNumber = cardNumber;
                _cardRepository.Add(card);
            }

            return card;
        }

        public BankOperationType GetOrAddOperationType(string typeName)
        {
            BankOperationType operationType = _bankOperationTypeRepository
                .GetAll().Where(a => a.Name == typeName || a.AlternativeNames == typeName)
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
