using Moq;
using MyBudget.Core.DataContext;
using MyBudget.Model;
using MyBudget.OperationsLoading;
using MyBudget.OperationsLoading.MilleniumAccount;
using MyBudget.OperationsLoading.Tests.Resources;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.OperationsLoading.Tests.MilleniumAccount
{
    [TestFixture]
    public class MilleniumParserTests
    {
        private Mock<IRepository<BankAccount, string>> accountRepo;
        private Mock<IRepository<BankOperationType, string>> typeRepo;
        private Mock<IRepository<Card, string>> cardRepo;
        private MilleniumParser parser;

        private List<BankAccount> mockAccountsCreated = new List<BankAccount>();

        [SetUp]
        public void SetUp()
        {
            this.accountRepo = new Mock<IRepository<BankAccount, string>>();
            this.typeRepo = new Mock<IRepository<BankOperationType, string>>();
            this.cardRepo = new Mock<IRepository<Card, string>>();
            this.parser = new MilleniumParser(new ParseHelper(), new RepositoryHelper(accountRepo.Object, typeRepo.Object, cardRepo.Object));

            this.accountRepo.Setup(a => a.Get(It.IsAny<string>())).Returns<string>(
                a => mockAccountsCreated.FirstOrDefault(x => x.Number == a));
            this.accountRepo.Setup(a => a.Add(It.IsAny<BankAccount>())).Callback<BankAccount>(
                a => mockAccountsCreated.Add(a));
        }

        [Test]
        public void GivenSampleCsvWithMultipleOperations_WhenParse_ThenListOfOperationsReturnedAnd2AccountsCreated()
        {
            //When
            var operations = this.parser.Parse(TestFiles.MilleniumParser_Sample);

            //Then
            Assert.AreEqual(7, operations.Count());
            VerifyOperations(operations);
            VerifyAccountAndCardNumbersAndOperationTypes(operations);
        }

        private static void VerifyOperations(IEnumerable<BankOperation> operations)
        {
            BankOperation checkOp;
            #region Account no 1

            //1
            checkOp = operations.FirstOrDefault(a =>
                a.OrderDate == new DateTime(2014, 09, 20) &&
                a.Amount == -1234.00M);

            Assert.IsNotNull(checkOp);
            Assert.AreEqual(checkOp.BankAccount.Number, TestBankData.Millenium_TestAccount1.Compact());
            Assert.AreEqual(checkOp.ExecutionDate, new DateTime(2014, 09, 20));
            Assert.AreEqual(checkOp.Type.Name, "PRZELEW WEWNĘTRZNY WYCHODZĄCY");
            Assert.AreEqual(checkOp.Cleared, true);
            Assert.AreEqual(checkOp.Description, "Przelew własny");
            Assert.AreEqual(checkOp.Title, "Przelew własny".GetFirstNCharacters(OperationsLoadingConsts.OperationTitleLength));
            Assert.AreEqual(checkOp.EndingBalance, 933.89M);
            Assert.AreEqual(checkOp.Card, null);
            Assert.IsTrue(string.IsNullOrWhiteSpace(checkOp.CounterAccount));
            Assert.AreEqual(checkOp.CounterParty, null);

            //2
            checkOp = operations.FirstOrDefault(a =>
                a.OrderDate == new DateTime(2014, 09, 19) &&
                a.Amount == -11.22M);

            Assert.IsNotNull(checkOp);
            Assert.AreEqual(checkOp.BankAccount.Number, TestBankData.Millenium_TestAccount1.Compact());
            Assert.AreEqual(checkOp.ExecutionDate, new DateTime(2014, 09, 19));
            Assert.AreEqual(checkOp.Type.Name, "OBCIĄŻENIE");
            Assert.AreEqual(checkOp.Cleared, true);
            Assert.AreEqual(checkOp.Description, "PODATEK OD ODSETEK");
            Assert.AreEqual(checkOp.Title, "PODATEK OD ODSETEK".GetFirstNCharacters(OperationsLoadingConsts.OperationTitleLength));
            Assert.AreEqual(checkOp.EndingBalance, 2167.89M);
            Assert.AreEqual(checkOp.Card, null);
            Assert.IsTrue(string.IsNullOrWhiteSpace(checkOp.CounterAccount));
            Assert.AreEqual(checkOp.CounterParty, null);

            //3
            checkOp = operations.FirstOrDefault(a =>
                a.OrderDate == new DateTime(2014, 09, 18) &&
                a.Amount == 55.66M);

            Assert.IsNotNull(checkOp);
            Assert.AreEqual(checkOp.BankAccount.Number, TestBankData.Millenium_TestAccount1.Compact());
            Assert.AreEqual(checkOp.ExecutionDate, new DateTime(2014, 09, 18));
            Assert.AreEqual(checkOp.Type.Name, "UZNANIE");
            Assert.AreEqual(checkOp.Cleared, true);
            Assert.AreEqual(checkOp.Description, "KAPITALIZACJA ODS.");
            Assert.AreEqual(checkOp.Title, "KAPITALIZACJA ODS.".GetFirstNCharacters(OperationsLoadingConsts.OperationTitleLength));
            Assert.AreEqual(checkOp.EndingBalance, 2179.11M);
            Assert.AreEqual(checkOp.Card, null);
            Assert.IsTrue(string.IsNullOrWhiteSpace(checkOp.CounterAccount));
            Assert.AreEqual(checkOp.CounterParty, null);

            //4
            checkOp = operations.FirstOrDefault(a =>
                a.OrderDate == new DateTime(2014, 09, 17) &&
                a.Amount == 123.45M);

            Assert.IsNotNull(checkOp);
            Assert.AreEqual(checkOp.BankAccount.Number, TestBankData.Millenium_TestAccount1.Compact());
            Assert.AreEqual(checkOp.ExecutionDate, new DateTime(2014, 09, 17));
            Assert.AreEqual(checkOp.Type.Name, "PRZELEW PRZYCHODZĄCY");
            Assert.AreEqual(checkOp.Cleared, true);
            Assert.AreEqual(checkOp.Description, "Bardzo dlugi tytul na ponad 30 znakow ktore trzeba bedzie przyciac");
            Assert.AreEqual(checkOp.Title, "Bardzo dlugi tytul na ponad 30 znakow ktore trzeba bedzie przyciac".GetFirstNCharacters(OperationsLoadingConsts.OperationTitleLength));
            Assert.AreEqual(checkOp.EndingBalance, 2123.45M);
            Assert.AreEqual(checkOp.Card, null);
            Assert.AreEqual(checkOp.CounterAccount, "11 22 2233 3344 4455 5566 6677 77".Compact());
            Assert.AreEqual(checkOp.CounterParty, null);

            #endregion

            #region Account no 2

            //5
            checkOp = operations.FirstOrDefault(a =>
                a.OrderDate == new DateTime(2014, 09, 14) &&
                a.Amount == -3.00M);

            Assert.IsNotNull(checkOp);
            Assert.AreEqual(checkOp.BankAccount.Number, TestBankData.Millenium_TestAccount2.Compact());
            Assert.AreEqual(checkOp.ExecutionDate, new DateTime(2014, 09, 16));
            Assert.AreEqual(checkOp.Type.Name, "TRANSAKCJA KARTĄ PŁATNICZĄ");
            Assert.AreEqual(checkOp.Cleared, true);
            Assert.AreEqual(checkOp.Description, "Trans");
            Assert.AreEqual(checkOp.Title, "Trans".GetFirstNCharacters(OperationsLoadingConsts.OperationTitleLength));
            Assert.AreEqual(checkOp.EndingBalance, 1990.37M);
            Assert.AreEqual(checkOp.Card, null);
            Assert.IsTrue(string.IsNullOrWhiteSpace(checkOp.CounterAccount));
            Assert.AreEqual(checkOp.CounterParty, null);

            //6
            checkOp = operations.FirstOrDefault(a =>
                a.OrderDate == new DateTime(2014, 09, 13) &&
                a.Amount == -3.52M);

            Assert.IsNotNull(checkOp);
            Assert.AreEqual(checkOp.BankAccount.Number, TestBankData.Millenium_TestAccount2.Compact());
            Assert.AreEqual(checkOp.ExecutionDate, new DateTime(2014, 09, 13));
            Assert.AreEqual(checkOp.Type.Name, "WYPŁATA KARTĄ Z BANKOMATU");
            Assert.AreEqual(checkOp.Cleared, true);
            Assert.AreEqual(checkOp.Description, "xx1");
            Assert.AreEqual(checkOp.Title, "xx1".GetFirstNCharacters(OperationsLoadingConsts.OperationTitleLength));
            Assert.AreEqual(checkOp.EndingBalance, 1993.37M);
            Assert.AreEqual(checkOp.Card, null);
            Assert.IsTrue(string.IsNullOrWhiteSpace(checkOp.CounterAccount));
            Assert.AreEqual(checkOp.CounterParty, null);

            //7
            checkOp = operations.FirstOrDefault(a =>
                a.OrderDate == new DateTime(2014, 09, 12) &&
                a.Amount == -3.11M);

            Assert.IsNotNull(checkOp);
            Assert.AreEqual(checkOp.BankAccount.Number, TestBankData.Millenium_TestAccount2.Compact());
            Assert.AreEqual(checkOp.ExecutionDate, new DateTime(2014, 09, 12));
            Assert.AreEqual(checkOp.Type.Name, "WYPŁATA KARTĄ Z BANKOMATU");
            Assert.AreEqual(checkOp.Cleared, true);
            Assert.AreEqual(checkOp.Description, "xx1");
            Assert.AreEqual(checkOp.Title, "xx1".GetFirstNCharacters(OperationsLoadingConsts.OperationTitleLength));
            Assert.AreEqual(checkOp.EndingBalance, 1996.89M);
            Assert.AreEqual(checkOp.Card, null);
            Assert.IsTrue(string.IsNullOrWhiteSpace(checkOp.CounterAccount));
            Assert.AreEqual(checkOp.CounterParty, null);


            #endregion

        }

        private void VerifyAccountAndCardNumbersAndOperationTypes(IEnumerable<BankOperation> operations)
        {
            //We want just 1 account of each type to be inserted
            accountRepo.Verify(repo => repo.Add(
                It.Is<BankAccount>(account =>
                account.Number == TestBankData.Millenium_TestAccount1.Compact())), Times.Once);
            accountRepo.Verify(repo => repo.Get(
                It.Is<string>(accountNumber => accountNumber == TestBankData.Millenium_TestAccount1.Compact())), Times.Exactly(4));

            accountRepo.Verify(repo => repo.Add(
                It.Is<BankAccount>(account =>
                account.Number == TestBankData.Millenium_TestAccount2.Compact())), Times.Once);
            accountRepo.Verify(repo => repo.Get(
                It.Is<string>(accountNumber => accountNumber == TestBankData.Millenium_TestAccount2.Compact())), Times.Exactly(3));

            typeRepo.Verify(repo => repo.Add(
                It.Is<BankOperationType>(type =>
                type.Name == "PRZELEW WEWNĘTRZNY WYCHODZĄCY")));
            typeRepo.Verify(repo => repo.Add(
                It.Is<BankOperationType>(type =>
                type.Name == "OBCIĄŻENIE")));
            typeRepo.Verify(repo => repo.Add(
                It.Is<BankOperationType>(type =>
                type.Name == "UZNANIE")));
            typeRepo.Verify(repo => repo.Add(
                It.Is<BankOperationType>(type =>
                type.Name == "PRZELEW PRZYCHODZĄCY")));
            typeRepo.Verify(repo => repo.Add(
                It.Is<BankOperationType>(type =>
                type.Name == "TRANSAKCJA KARTĄ PŁATNICZĄ")));
            typeRepo.Verify(repo => repo.Add(
                It.Is<BankOperationType>(type =>
                type.Name == "WYPŁATA KARTĄ Z BANKOMATU")));
        }

        [Test]
        public void GivenSampleCsvWithOneOperation_WhenParsed_ThenSingleOperationParsedWithProperValuesInFields()
        {
            //When
            var operations = this.parser.Parse(TestFiles.MilleniumParser_Sample1Entry);

            //Then
            var op = operations.Single();
            Assert.AreEqual(TestBankData.Millenium_TestAccount1.Compact(), op.BankAccount.Number);
            Assert.AreEqual(new DateTime(2014, 9, 17), op.OrderDate);
            Assert.AreEqual(new DateTime(2014, 9, 17), op.ExecutionDate);
            Assert.AreEqual(123.45, op.Amount);
            Assert.AreEqual("PRZELEW PRZYCHODZĄCY", op.Type.Name);
            Assert.AreEqual(true, op.Cleared);
            Assert.AreEqual("Tytul", op.Description);
            Assert.AreEqual(TestBankData.ExternalAccount_TestAccount1.Compact(), op.CounterAccount);
        }

        [Test]
        public void GivenSampleMillniumCSVWithComma_WhenParsed_ThenOperationIsParsedCorrectly()
        {
            //When
            var operations = this.parser.Parse(TestFiles.MilleniumParser_Sample1Entry_Comma);

            //Then
            var op = operations.Single();
            Assert.AreEqual(TestBankData.Millenium_TestAccount1.Compact(), op.BankAccount.Number);
            Assert.AreEqual(new DateTime(2014, 9, 17), op.OrderDate);
            Assert.AreEqual(new DateTime(2014, 9, 17), op.ExecutionDate);
            Assert.AreEqual(123.45, op.Amount);
            Assert.AreEqual("PRZELEW PRZYCHODZĄCY", op.Type.Name);
            Assert.AreEqual(true, op.Cleared);
            Assert.AreEqual("Tytul, z przecinkiem", op.Description);
            Assert.AreEqual(TestBankData.ExternalAccount_TestAccount1.Compact(), op.CounterAccount);
        }
    }
}
