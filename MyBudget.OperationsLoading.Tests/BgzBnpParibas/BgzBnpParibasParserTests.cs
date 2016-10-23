using Moq;
using MyBudget.Core.DataContext;
using MyBudget.Model;
using MyBudget.OperationsLoading.BgzBnpParibas;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.OperationsLoading.Tests.BgzBnpParibas
{
    [TestFixture]
    public class BgzBnpParibasParserTests
    {
        private Mock<IRepository<BankAccount, string>> accountRepo;
        private Mock<IRepository<BankOperationType, string>> typeRepo;
        private Mock<IRepository<Card, string>> cardRepo;
        private BgzBnpParibasParser parser;

        [SetUp]
        public void SetUp()
        {
            this.accountRepo = new Mock<IRepository<BankAccount, string>>();
            this.typeRepo = new Mock<IRepository<BankOperationType, string>>();
            this.cardRepo = new Mock<IRepository<Card, string>>();
            this.parser = new BgzBnpParibasParser();
        }

        [Test]
        [SetUICulture("pl")]
        public void BgzParser_FilterCSVFilesPL()
        {
            //When            
            var extensions = this.parser.SupportedFileExtensions;

            //Then
            Assert.AreEqual("BGZ BNP Paribas" + " " + "(.csv)|*.csv", extensions);
        }

        [Test]
        [SetUICulture("en")]
        public void BgzParser_FilterCSVFilesEN()
        {
            //When            
            var extensions = this.parser.SupportedFileExtensions;

            //Then
            Assert.AreEqual("BGZ BNP Paribas operations" + " " + "(.csv)|*.csv", extensions);
        }


        [Test]
        public void GivenBasicCases_WhenParseBgzFormat_Then5OperationsAreReturnedAccountAnd5NewTypesAreAdded()
        {
            //When
            var operations = parser.Parse(Resources.TestFiles.BGZParser_StandardCases);

            //Then
            Assert.AreEqual(5, operations.Count());
            VerifyOperations(operations);
            VerifyAccountAndOperationTypes();
        }

        private static void VerifyOperations(IEnumerable<BankOperation> operations)
        {
            operations.Any(op =>
                op.BankAccount.Name == "BGZBNPParibas" &&
                op.ExecutionDate == new DateTime(2016, 10, 1) &&
                op.OrderDate == new DateTime(2016, 10, 1) &&
                op.Amount == 1000.12M &&
                op.Type.Name == "PRZELEW PRZYCHODZĄCY" &&
                op.Cleared == true &&
                op.Description == "ABC WYPŁATA 01 10 2016    ABC. Z O.O.   UL.ABC 1 11-111 WARSZAWA  01 2345 6789 0123 4567 8901 2345 ABC CR/Aaaa" &&
                op.EndingBalance == 1000.12M &&
                op.CounterAccount == "01234567890123456789012345");

            operations.Any(op =>
                op.BankAccount.Name == "BGZBNPParibas" &&
                op.ExecutionDate == new DateTime(2016, 10, 2) &&
                op.OrderDate == new DateTime(2016, 10, 2) &&
                op.Amount == -100.56M &&
                op.Type.Name == "PRZELEW" &&
                op.Cleared == true &&
                op.Description == "Abc" &&
                op.EndingBalance == 899.56M &&
                op.CounterAccount == "01234567890123456789012346");

            operations.Any(op =>
                op.BankAccount.Name == "BGZBNPParibas" &&
                op.ExecutionDate == new DateTime(2016, 10, 3) &&
                op.OrderDate == new DateTime(2016, 10, 3) &&
                op.Amount == -4.00M &&
                op.Type.Name == "PRZELEW DO INNEGO BANKU" &&
                op.Cleared == true &&
                op.Description == "asadfasfdsaf" &&
                op.EndingBalance == 895.56M &&
                op.CounterAccount == "01234567890123456789012347");

            operations.Any(op =>
                op.BankAccount.Name == "BGZBNPParibas" &&
                op.ExecutionDate == new DateTime(2016, 10, 5) &&
                op.OrderDate == new DateTime(2016, 10, 4) &&
                op.Amount == -10.11M &&
                op.Type.Name == "WYPŁATA KARTĄ Z BANKOMATU" &&
                op.Cleared == true &&
                op.Description == "WYPŁATA KARTĄ Z BANKOMATU A111 BANK1 SA" &&
                op.EndingBalance == 885.45M &&
                op.Card.CardNumber == "123456XXXXXX7890");

            operations.Any(op =>
                op.BankAccount.Name == "BGZBNPParibas" &&
                op.ExecutionDate == new DateTime(2016, 10, 7) &&
                op.OrderDate == new DateTime(2016, 10, 4) &&
                op.Amount == -23.00M &&
                op.Type.Name == "TRANSAKCJA KARTĄ PŁATNICZĄ" &&
                op.Cleared == true &&
                op.Description == "SKLEP SPOZYWCZY" &&
                op.EndingBalance == 862.45M &&
                op.Card.CardNumber == "123456XXXXXX7891");
        }

        private void VerifyAccountAndOperationTypes()
        {
            accountRepo.Verify(repo => repo.Add(
                It.Is<BankAccount>(account =>
                account.Name == "BGZBNPParibas" &&
                string.IsNullOrEmpty(account.Description))));

            cardRepo.Verify(repo => repo.Add(
                It.Is<Card>(card =>
                card.CardNumber == "123456XXXXXX7890")));

            cardRepo.Verify(repo => repo.Add(
                It.Is<Card>(card =>
                card.CardNumber == "123456XXXXXX7891")));

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
            var operations = parser.Parse(Resources.TestFiles.BGZParser_StandardCases);

            //Then
            Assert.AreEqual(1, operations.Count());
            operations.Any(op =>
                op.BankAccount.Name == "BGZBNPParibas" &&
                op.ExecutionDate == new DateTime(2016, 10, 8) &&
                op.OrderDate == new DateTime(2016, 10, 4) &&
                op.Amount == -24.000M &&
                op.Type.Name == "TRANSAKCJA KARTĄ PŁATNICZĄ" &&
                op.Cleared == true &&
                op.Description == "MORETHAN15SIGNSDESCSPACE" &&
                op.EndingBalance == 862.45M &&
                op.Card.CardNumber == "123456XXXXXX7891");
        }

        [Test]
        public void GivenOtherTransaction_WhenParseBgzFormat_ThenGenericTypeOperationAdded()
        {
            //To add test for operations that were not analysed/designed yet
            Assert.Inconclusive();
        }
    }
}
