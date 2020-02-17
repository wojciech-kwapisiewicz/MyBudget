using Moq;
using MyBudget.Core.DataContext;
using MyBudget.Model;
using MyBudget.OperationsLoading.BnpParibasXlsx;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyBudget.OperationsLoading.Tests.BnpParibasXlsx
{
    [TestFixture]
    public class BnpParibasXslxParserTests
    {
        private Mock<IRepository<BankAccount, string>> accountRepo;
        private Mock<IRepository<BankOperationType, string>> typeRepo;
        private Mock<IRepository<Card, string>> cardRepo;
        private RepositoryHelper repositoryHelper;
        private BnpParibasXslxParser parser;
        private ParseHelper parseHelper = new ParseHelper();

        [SetUp]
        public void SetUp()
        {
            this.accountRepo = new Mock<IRepository<BankAccount, string>>();
            this.typeRepo = new Mock<IRepository<BankOperationType, string>>();
            this.cardRepo = new Mock<IRepository<Card, string>>();
            this.repositoryHelper = new RepositoryHelper(accountRepo.Object, typeRepo.Object, cardRepo.Object);
            this.parser = new BnpParibasXslxParser(parseHelper, repositoryHelper);
        }

        [Test]
        [SetUICulture("pl")]
        public void BgzParser_FilterCSVFilesPL()
        {
            //When            
            var extensions = this.parser.SupportedFileExtensions;

            //Then
            Assert.AreEqual("BNPParibas (XLS / Excel) (.xlsx)|*.xlsx", extensions);
        }

        [Test]
        [SetUICulture("en")]
        public void BgzParser_FilterCSVFilesEN()
        {
            //When            
            var extensions = this.parser.SupportedFileExtensions;

            //Then
            Assert.AreEqual("BNPParibas operations (XLS / Excel) (.xlsx)|*.xlsx", extensions);
        }

        [Test]
        public void GivenBasicCases_WhenParseBnpFormat_Then10OperationsAreReturned_AccountAnd8NewTypesAreAdded()
        {
            //When                
            var operations = parser.Parse(Resources.TestFiles.BNP_TestOperations.ToStream());

            //Then
            Assert.AreEqual(10, operations.Count());
            VerifyOperations(operations);
            VerifyAccountAndCardNumbersAndOperationTypes();
        }

        private static void VerifyOperations(IEnumerable<BankOperation> operations)
        {
            #region Operation type - default

            Assert.IsTrue(operations.Any(op =>
                op.BankAccount.Number == TestBankData.BNPParibas_TestAccount1.Compact() &&
                op.ExecutionDate == new DateTime(2020, 02, 17) &&
                op.OrderDate == new DateTime(2020, 02, 17) &&
                op.Amount == -1000.00M &&
                op.Type.Name == "Operacja kredytowa" &&
                op.Cleared == true &&
                op.Description == "Operacja kredytowa nr 01234568" &&
                op.Title == "Operacja kredytowa n" &&
                op.EndingBalance == 0.0M &&
                op.Card == null &&
                op.CounterAccount == string.Empty));

            Assert.IsTrue(operations.Any(op =>
                op.BankAccount.Number == TestBankData.BNPParibas_TestAccount1.Compact() &&
                op.ExecutionDate == new DateTime(2020, 02, 17) &&
                op.OrderDate == new DateTime(2020, 02, 17) &&
                op.Amount == -1200.00M &&
                op.Type.Name == "Prowizje i opłaty" &&
                op.Cleared == true &&
                op.Description == "Prowizja kredytowa nr 01234567" &&
                op.Title == "Prowizja kredytowa n" &&
                op.EndingBalance == 0.0M &&
                op.Card == null &&
                op.CounterAccount == string.Empty));

            #endregion

            #region Operation type - bank transfer

            Assert.IsTrue(operations.Any(op =>
                op.BankAccount.Number == TestBankData.BNPParibas_TestAccount1.Compact() &&
                op.ExecutionDate == new DateTime(2020, 02, 21) &&
                op.OrderDate == new DateTime(2020, 02, 21) &&
                op.Amount == 2345.00M &&
                op.Type.Name == "Przelew przychodzący" &&
                op.Cleared == true &&
                op.Description == "Przelew przychodzacy - wyplata" &&
                op.Title == "Przelew przychodzacy" &&
                op.EndingBalance == 0.0M &&
                op.Card == null &&
                op.CounterAccount == TestBankData.ExternalAccount_TestAccount1.Compact()));

            Assert.IsTrue(operations.Any(op =>
                op.BankAccount.Number == TestBankData.BNPParibas_TestAccount1.Compact() &&
                op.ExecutionDate == new DateTime(2020, 02, 20) &&
                op.OrderDate == new DateTime(2020, 02, 20) &&
                op.Amount == -300.00M &&
                op.Type.Name == "Przelew wychodzący" &&
                op.Cleared == true &&
                op.Description == "Oplata za cos" &&
                op.Title == "Oplata za cos" &&
                op.EndingBalance == 0.0M &&
                op.Card == null &&
                op.CounterAccount == TestBankData.ExternalAccount_TestAccount1.Compact()));

            Assert.IsTrue(operations.Any(op =>
                op.BankAccount.Number == TestBankData.BNPParibas_TestAccount1.Compact() &&
                op.ExecutionDate == new DateTime(2020, 02, 19) &&
                op.OrderDate == new DateTime(2020, 02, 19) &&
                op.Amount == -150.00M &&
                op.Type.Name == "Zlecenie stałe" &&
                op.Cleared == true &&
                op.Description == "Oplata stala za prad" &&
                op.Title == "Oplata stala za prad" &&
                op.EndingBalance == 0.0M &&
                op.Card == null &&
                op.CounterAccount == TestBankData.ExternalAccount_TestAccount2.Compact()));

            Assert.IsTrue(operations.Any(op =>
                op.BankAccount.Number == TestBankData.BNPParibas_TestAccount1.Compact() &&
                op.ExecutionDate == new DateTime(2020, 02, 10) &&
                op.OrderDate == new DateTime(2020, 02, 10) &&
                op.Amount == -120.00M &&
                op.Type.Name == "Przelew internetowy" &&
                op.Cleared == true &&
                op.Description == "Numer 12345678901234567890" &&
                op.Title == "Numer 12345678901234" &&
                op.EndingBalance == 0.0M &&
                op.Card == null &&
                op.CounterAccount == TestBankData.ExternalAccount_TestAccount1.Compact()));

            #endregion

            #region Operation type - blik

            Assert.IsTrue(operations.Any(op =>
                op.BankAccount.Number == TestBankData.BNPParibas_TestAccount1.Compact() &&
                op.ExecutionDate == new DateTime(2020, 02, 16) &&
                op.OrderDate == new DateTime(2020, 02, 16) &&
                op.Amount == 50.00M &&
                op.Type.Name == "Transakcja BLIK" &&
                op.Cleared == true &&
                op.Description == "Transakcja BLIK, Zwrot BLIK, Zwrot BLIK internet, Nr 12345678901, Zwrot za zakupy" &&
                op.Title == "Zwrot BLIK, Zwrot BL" &&
                op.EndingBalance == 0.0M &&
                op.Card.CardNumber == string.Empty &&
                op.CounterAccount == TestBankData.ExternalAccount_TestAccount1.Compact()));

            Assert.IsTrue(operations.Any(op =>
                op.BankAccount.Number == TestBankData.BNPParibas_TestAccount1.Compact() &&
                op.ExecutionDate == new DateTime(2020, 02, 11) &&
                op.OrderDate == new DateTime(2020, 02, 11) &&
                op.Amount == -300.00M &&
                op.Type.Name == "Transakcja BLIK" &&
                op.Cleared == true &&
                op.Description == "Transakcja BLIK, 12345678901, Platnosc za zakupy" &&
                op.Title == "12345678901, Platnos" &&
                op.EndingBalance == 0.0M &&
                op.Card.CardNumber == string.Empty &&
                op.CounterAccount == TestBankData.ExternalAccount_TestAccount1.Compact()));

            #endregion

            #region Operation type - card transaction

            Assert.IsTrue(operations.Any(op =>
                op.BankAccount.Number == TestBankData.BNPParibas_TestAccount1.Compact() &&
                op.ExecutionDate == new DateTime(2020, 02, 25) &&
                op.OrderDate == new DateTime(2020, 02, 25) &&
                op.Amount == -2.40M &&
                op.Type.Name == "Transakcja kartą" &&
                op.Cleared == true &&
                op.Description == "512345------0010 ANNA KOWALSKA WARSZAWA MPK1234 1WRO URBANCARD POL 2,40 PLN 2020-02-25" &&
                op.Title == "WARSZAWA MPK1234 1WR" &&
                op.EndingBalance == 0.0M &&
                op.Card.CardNumber == TestBankData.CardNo1));

            Assert.IsTrue(operations.Any(op =>
                op.BankAccount.Number == TestBankData.BNPParibas_TestAccount1.Compact() &&
                op.ExecutionDate == new DateTime(2020, 02, 24) &&
                op.OrderDate == new DateTime(2020, 02, 23) &&
                op.Amount == -234.00M &&
                op.Type.Name == "Transakcja kartą" &&
                op.Cleared == true &&
                op.Description == "512345------0020 JAN KOWALSKI WARSZAWA SKLEP WARSZAWA POL 234,00 PLN 2020-02-24" &&
                op.Title == "WARSZAWA SKLEP WARSZ" &&
                op.EndingBalance == 0.0M &&
                op.Card.CardNumber == TestBankData.CardNo2));

            #endregion
        }

        private void VerifyAccountAndCardNumbersAndOperationTypes()
        {
            accountRepo.Verify(repo => repo.Add(
                It.Is<BankAccount>(account =>
                account.Number == TestBankData.BNPParibas_TestAccount1)));

            cardRepo.Verify(repo => repo.Add(
                It.Is<Card>(card =>
                card.CardNumber == TestBankData.CardBNP_No1)));

            cardRepo.Verify(repo => repo.Add(
                It.Is<Card>(card =>
                card.CardNumber == TestBankData.CardBNP_No2)));

            typeRepo.Verify(repo => repo.Add(
                It.Is<BankOperationType>(type =>
                type.Name == "Transakcja kartą")));
            typeRepo.Verify(repo => repo.Add(
                It.Is<BankOperationType>(type =>
                type.Name == "Przelew przychodzący")));
            typeRepo.Verify(repo => repo.Add(
                It.Is<BankOperationType>(type =>
                type.Name == "Przelew wychodzący")));
            typeRepo.Verify(repo => repo.Add(
                It.Is<BankOperationType>(type =>
                type.Name == "Zlecenie stałe")));
            typeRepo.Verify(repo => repo.Add(
                It.Is<BankOperationType>(type =>
                type.Name == "Operacja kredytowa")));
            typeRepo.Verify(repo => repo.Add(
                It.Is<BankOperationType>(type =>
                type.Name == "Prowizje i opłaty")));
            typeRepo.Verify(repo => repo.Add(
                It.Is<BankOperationType>(type =>
                type.Name == "Transakcja BLIK")));
            typeRepo.Verify(repo => repo.Add(
                It.Is<BankOperationType>(type =>
                type.Name == "Przelew internetowy")));
        }
    }
}
