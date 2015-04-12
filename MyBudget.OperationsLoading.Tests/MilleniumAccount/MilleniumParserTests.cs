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

namespace MyBudget.Core.UnitTests.ImportData
{
    [TestFixture]
    public class MilleniumParserTests
    {
        private Mock<IRepository<BankAccount, string>> accountRepo;
        private Mock<IRepository<BankOperationType, string>> typeRepo;
        private MilleniumParser parser;

        [SetUp]
        public void SetUp()
        {
            this.accountRepo = new Mock<IRepository<BankAccount, string>>();
            this.typeRepo = new Mock<IRepository<BankOperationType, string>>();
            this.parser = new MilleniumParser(new ParseHelper(), new RepositoryHelper(accountRepo.Object, typeRepo.Object));
        }

        [Test]
        public void GivenSampleMilleniumCsvWithMultipleOperations_WhenParse_ThenListOfOperationsReturnedAnd2AccountsCreated()
        {
            //When
            var list = this.parser.Parse(TestFiles.MilleniumParser_Sample);

            //Then
            Assert.AreEqual(7, list.Count());            
        }

        [Test]
        public void GivenSampleMilleniumCsvWithOneOperation_WhenParsed_ThenSingleOperationParsedWithProperValuesInFields()
        {
            //When
            var list = this.parser.Parse(TestFiles.MilleniumParser_Sample1Entry);

            //Then
            var op = list.Single();
            Assert.AreEqual("00012345678910000011111234", op.BankAccount.Number);
            Assert.AreEqual(new DateTime(2014, 9, 17), op.OrderDate);
            Assert.AreEqual(new DateTime(2014, 9, 17), op.ExecutionDate);
            Assert.AreEqual(123.45, op.Amount);
            Assert.AreEqual("PRZELEW PRZYCHODZĄCY", op.Type.Name);
            Assert.AreEqual(true, op.Cleared);
            Assert.AreEqual("Tytul", op.Description);
            Assert.AreEqual("03102039580000123456789000", op.CounterAccount);
        }
    }
}
