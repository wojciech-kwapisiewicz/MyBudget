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
            #region Account no 1

            Assert.IsTrue(operations.Any(op =>
                op.BankAccount.Number == TestBankData.Millenium_TestAccount1.Compact() &&
                op.ExecutionDate == new DateTime(2014, 09, 20) &&
                op.OrderDate == new DateTime(2014, 09, 20) &&
                op.Amount == -1234.00M &&
                op.Type.Name == "PRZELEW WEWNĘTRZNY WYCHODZĄCY" &&
                op.Cleared == true &&
                op.Description == "Przelew własny" &&
                op.Title == "Przelew własny".GetFirstNCharacters(OperationsLoadingConsts.OperationTitleLength) &&
                op.EndingBalance == 933.89M &&
                op.Card == null &&
                string.IsNullOrEmpty(op.CounterAccount)));

            Assert.IsTrue(operations.Any(op =>
                op.BankAccount.Number == TestBankData.Millenium_TestAccount1.Compact() &&
                op.ExecutionDate == new DateTime(2014, 09, 19) &&
                op.OrderDate == new DateTime(2014, 09, 19) &&
                op.Amount == -11.22M &&
                op.Type.Name == "OBCIĄŻENIE" &&
                op.Cleared == true &&
                op.Description == "PODATEK OD ODSETEK" &&
                op.Title == "PODATEK OD ODSETEK".GetFirstNCharacters(OperationsLoadingConsts.OperationTitleLength) &&
                op.EndingBalance == 2167.89M &&
                op.Card == null &&
                string.IsNullOrEmpty(op.CounterAccount)));

            Assert.IsTrue(operations.Any(op =>
                op.BankAccount.Number == TestBankData.Millenium_TestAccount1.Compact() &&
                op.ExecutionDate == new DateTime(2014, 09, 18) &&
                op.OrderDate == new DateTime(2014, 09, 18) &&
                op.Amount == 55.66M &&
                op.Type.Name == "UZNANIE" &&
                op.Cleared == true &&
                op.Description == "KAPITALIZACJA ODS." &&
                op.Title == "KAPITALIZACJA ODS.".GetFirstNCharacters(OperationsLoadingConsts.OperationTitleLength) &&
                op.EndingBalance == 2179.11M &&
                op.Card == null &&
                string.IsNullOrEmpty(op.CounterAccount)));

            Assert.IsTrue(operations.Any(op =>
                op.BankAccount.Number == TestBankData.Millenium_TestAccount1.Compact() &&
                op.ExecutionDate == new DateTime(2014, 09, 17) &&
                op.OrderDate == new DateTime(2014, 09, 17) &&
                op.Amount == 123.45M &&
                op.Type.Name == "PRZELEW PRZYCHODZĄCY" &&
                op.Cleared == true &&
                op.Description == "Bardzo dlugi tytul na ponad 30 znakow ktore trzeba bedzie przyciac" &&
                op.Title == "Bardzo dlugi tytul na ponad 30 znakow ktore trzeba bedzie przyciac".GetFirstNCharacters(OperationsLoadingConsts.OperationTitleLength) &&
                op.EndingBalance == 2123.45M &&
                op.Card == null &&
                op.CounterAccount == "11 22 2233 3344 4455 5566 6677 77".Compact()));

            #endregion

            #region Account no 2

            Assert.IsTrue(operations.Any(op =>
                op.BankAccount.Number == TestBankData.Millenium_TestAccount2.Compact() &&
                op.ExecutionDate == new DateTime(2014, 09, 16) &&
                op.OrderDate == new DateTime(2014, 09, 14) &&
                op.Amount == -3.00M &&
                op.Type.Name == "TRANSAKCJA KARTĄ PŁATNICZĄ" &&
                op.Cleared == true &&
                op.Description == "Trans" &&
                op.Title == "Trans".GetFirstNCharacters(OperationsLoadingConsts.OperationTitleLength) &&
                op.EndingBalance == 1990.37M &&
                op.Card == null &&
                string.IsNullOrEmpty(op.CounterAccount)));

            Assert.IsTrue(operations.Any(op =>
                op.BankAccount.Number == TestBankData.Millenium_TestAccount2.Compact() &&
                op.ExecutionDate == new DateTime(2014, 09, 13) &&
                op.OrderDate == new DateTime(2014, 09, 13) &&
                op.Amount == -3.52M &&
                op.Type.Name == "WYPŁATA KARTĄ Z BANKOMATU" &&
                op.Cleared == true &&
                op.Description == "xx1" &&
                op.Title == "xx1".GetFirstNCharacters(OperationsLoadingConsts.OperationTitleLength) &&
                op.EndingBalance == 1993.37M &&
                op.Card == null &&
                string.IsNullOrEmpty(op.CounterAccount)));

            Assert.IsTrue(operations.Any(op =>
                op.BankAccount.Number == TestBankData.Millenium_TestAccount2.Compact() &&
                op.ExecutionDate == new DateTime(2014, 09, 12) &&
                op.OrderDate == new DateTime(2014, 09, 12) &&
                op.Amount == -3.11M &&
                op.Type.Name == "WYPŁATA KARTĄ Z BANKOMATU" &&
                op.Cleared == true &&
                op.Description == "xx1" &&
                op.Title == "xx1".GetFirstNCharacters(OperationsLoadingConsts.OperationTitleLength) &&
                op.EndingBalance == 1996.89M &&
                op.Card == null &&
                string.IsNullOrEmpty(op.CounterAccount)));

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
