using Moq;
using MyBudget.Core.DataContext;
using MyBudget.Model;
using MyBudget.OperationsLoading.BgzBnpParibasCsv;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.OperationsLoading.Tests.BgzBnpParibasCsv
{
    [TestFixture]
    public class BgzBnpParibasParserTests
    {
        private Mock<IRepository<BankAccount, string>> accountRepo;
        private Mock<IRepository<BankOperationType, string>> typeRepo;
        private Mock<IRepository<Card, string>> cardRepo;
        private RepositoryHelper repositoryHelper;
        private BgzBnpParibasCsvParser parser;
        private ParseHelper parseHelper = new ParseHelper();

        [SetUp]
        public void SetUp()
        {
            this.accountRepo = new Mock<IRepository<BankAccount, string>>();
            this.typeRepo = new Mock<IRepository<BankOperationType, string>>();
            this.cardRepo = new Mock<IRepository<Card, string>>();
            this.repositoryHelper = new RepositoryHelper(accountRepo.Object, typeRepo.Object, cardRepo.Object);
            IFillOperationFromDescriptionChain chain =
                new WyplataBankomat(repositoryHelper, parseHelper,
                new OperacjaKarta(repositoryHelper, parseHelper,
                new Przelew(repositoryHelper,
                new PrzelewPrzychodzacy(repositoryHelper, parseHelper,
                new PrzelewWychodzacy(repositoryHelper,
                new InnaOperacja(repositoryHelper))))));
            this.parser = new BgzBnpParibasCsvParser(
                parseHelper, repositoryHelper, new WyplataBankomat(repositoryHelper, parseHelper, chain));
        }

        [Test]
        [SetUICulture("pl")]
        public void BgzParser_FilterCSVFilesPL()
        {
            //When            
            var extensions = this.parser.SupportedFileExtensions;

            //Then
            Assert.AreEqual("[Wycofany] BGZ BNP Paribas (CSV)" + " " + "(.csv)|*.csv", extensions);
        }

        [Test]
        [SetUICulture("en")]
        public void BgzParser_FilterCSVFilesEN()
        {
            //When            
            var extensions = this.parser.SupportedFileExtensions;

            //Then
            Assert.AreEqual("[Obsolete] BGZ BNP Paribas operations (CSV)" + " " + "(.csv)|*.csv", extensions);
        }


        [Test]
        public void GivenBasicCases_WhenParseBgzFormat_Then5OperationsAreReturnedAccountAnd5NewTypesAreAdded()
        {
            //When                
            var operations = parser.Parse(Resources.TestFiles.BGZParser_StandardCases.ToStream());

            //Then
            Assert.AreEqual(5, operations.Count());
            VerifyOperations(operations);
            VerifyAccountAndCardNumbersAndOperationTypes();
        }

        private static void VerifyOperations(IEnumerable<BankOperation> operations)
        {
            Assert.IsTrue(operations.Any(op =>
                op.BankAccount.Number == "BGZBNPParibas" &&
                op.ExecutionDate == new DateTime(2016, 10, 1) &&
                op.OrderDate == new DateTime(2016, 10, 1) &&
                op.Amount == 1000.12M &&
                op.Type.Name == "PRZELEW PRZYCHODZĄCY" &&
                op.Cleared == true &&
                op.Description == $"ABC WYPLATA 01 10 2016    ABC. Z O.O.   UL.ABC 1 11-111 WARSZAWA  {TestBankData.ExternalAccount_TestAccount1} ABC CR/Aaaa" &&
                op.Title == "ABC WYPLATA 01 10 20" &&
                op.EndingBalance == 1000.12M &&
                op.CounterAccount == TestBankData.ExternalAccount_TestAccount1.Compact()));

            Assert.IsTrue(operations.Any(op =>
                op.BankAccount.Number == "BGZBNPParibas" &&
                op.ExecutionDate == new DateTime(2016, 10, 2) &&
                op.OrderDate == new DateTime(2016, 10, 2) &&
                op.Amount == -100.56M &&
                op.Type.Name == "PRZELEW" &&
                op.Cleared == true &&
                op.Description == "Abc" &&
                op.Title == "Abc" &&
                op.EndingBalance == 899.56M &&
                op.CounterAccount == TestBankData.ExternalAccount_TestAccount1.Compact()));

            Assert.IsTrue(operations.Any(op =>
                op.BankAccount.Number == "BGZBNPParibas" &&
                op.ExecutionDate == new DateTime(2016, 10, 3) &&
                op.OrderDate == new DateTime(2016, 10, 3) &&
                op.Amount == -4.00M &&
                op.Type.Name == "PRZELEW DO INNEGO BANKU" &&
                op.Cleared == true &&
                op.Description == "asadfasfdsaf" &&
                op.Title == "asadfasfdsaf" &&
                op.EndingBalance == 895.56M &&
                op.CounterAccount == TestBankData.ExternalAccount_TestAccount1.Compact()));

            Assert.IsTrue(operations.Any(op =>
                op.BankAccount.Number == "BGZBNPParibas" &&
                op.ExecutionDate == new DateTime(2016, 10, 5) &&
                op.OrderDate == new DateTime(2016, 10, 4) &&
                op.Amount == -10.11M &&
                op.Type.Name == "WYPŁATA KARTĄ Z BANKOMATU" &&
                op.Cleared == true &&
                op.Description == "WYPŁATA KARTĄ Z BANKOMATU A111 BANK1 SA" &&
                op.Title == "WYPŁATA KARTĄ Z BANK" &&
                op.EndingBalance == 885.45M &&
                op.Card.CardNumber == TestBankData.CardNo1));

            Assert.IsTrue(operations.Any(op =>
                op.BankAccount.Number == "BGZBNPParibas" &&
                op.ExecutionDate == new DateTime(2016, 10, 7) &&
                op.OrderDate == new DateTime(2016, 10, 4) &&
                op.Amount == -23.00M &&
                op.Type.Name == "TRANSAKCJA KARTĄ PŁATNICZĄ" &&
                op.Cleared == true &&
                op.Description == "SKLEP SPOZYWCZY" &&
                op.Title == "SKLEP SPOZYWCZY" &&
                op.EndingBalance == 862.45M &&
                op.Card.CardNumber == TestBankData.CardNo2));
        }

        private void VerifyAccountAndCardNumbersAndOperationTypes()
        {
            accountRepo.Verify(repo => repo.Add(
                It.Is<BankAccount>(account =>
                account.Number == "BGZBNPParibas")));

            cardRepo.Verify(repo => repo.Add(
                It.Is<Card>(card =>
                card.CardNumber == TestBankData.CardNo1)));

            cardRepo.Verify(repo => repo.Add(
                It.Is<Card>(card =>
                card.CardNumber == TestBankData.CardNo2)));

            typeRepo.Verify(repo => repo.Add(
                It.Is<BankOperationType>(type =>
                type.Name == "WYPŁATA KARTĄ Z BANKOMATU")));
            typeRepo.Verify(repo => repo.Add(
                It.Is<BankOperationType>(type =>
                type.Name == "TRANSAKCJA KARTĄ PŁATNICZĄ")));
            typeRepo.Verify(repo => repo.Add(
                It.Is<BankOperationType>(type =>
                type.Name == "PRZELEW")));
            typeRepo.Verify(repo => repo.Add(
                It.Is<BankOperationType>(type =>
                type.Name == "PRZELEW PRZYCHODZĄCY")));
            typeRepo.Verify(repo => repo.Add(
                It.Is<BankOperationType>(type =>
                type.Name == "PRZELEW DO INNEGO BANKU")));
        }

        [Test]
        public void GivenTransactionWithDescriptionLongerThan15chars_WhenParseBgzFormat_ThenSpaceafter15charIsRemoved()
        {
            //There is interesting "feature" that after 15th case in BGZ BNP Paribas CSV exports there is extra space (" ") needlessly added
            //Probably this is newline replaced by space

            //When
            var operations = parser.Parse(Resources.TestFiles.BGZParser_LongDescPayment.ToStream());

            //Then
            Assert.AreEqual(1, operations.Count());
            Assert.IsTrue(operations.Any(op =>
                op.BankAccount.Number == "BGZBNPParibas" &&
                op.ExecutionDate == new DateTime(2016, 10, 8) &&
                op.OrderDate == new DateTime(2016, 10, 4) &&
                op.Amount == -24.00M &&
                op.Type.Name == "TRANSAKCJA KARTĄ PŁATNICZĄ" &&
                op.Cleared == true &&
                op.Description == "MORETHAN15SIGNSDESCSPACE" &&
                op.EndingBalance == 862.45M &&
                op.Card.CardNumber == TestBankData.CardNo2));
        }

        [Test]
        public void GivenOtherTransaction_WhenParseBgzFormat_ThenGenericTypeOperationAdded()
        {
            //To add test for operations that were not analysed/designed yet
            //When
            var operations = parser.Parse(Resources.TestFiles.BGZParser_OtherOperation.ToStream());

            //Then
            Assert.AreEqual(1, operations.Count());
            Assert.IsTrue(operations.Any(op =>
                op.BankAccount.Number == "BGZBNPParibas" &&
                op.ExecutionDate == new DateTime(2016, 10, 10) &&
                op.OrderDate == new DateTime(2016, 10, 10) &&
                op.Amount == -345.45M &&
                op.Type.Name == "INNA OPERACJA" &&
                op.Cleared == true &&
                op.Description == "blablba" &&
                op.EndingBalance == 345.45M));
        }
    }
}
