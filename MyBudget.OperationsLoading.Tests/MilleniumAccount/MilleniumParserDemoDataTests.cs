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

namespace MyBudget.OperationsLoading.Tests.MilleniumAccount
{
    [TestFixture]
    public class MilleniumParserDemoDataTests
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

            this.accountRepo.Setup(a => a.Get(It.IsAny<string>())).Returns<string>(a => mockAccountsCreated.FirstOrDefault(x => x.Number == a));
            this.accountRepo.Setup(a => a.Add(It.IsAny<BankAccount>())).Callback<BankAccount>(a => mockAccountsCreated.Add(a));
        }

        [Test]
        public void GivenDemoCsv03_WhenParsed_ThenOperationsAreLoaded()
        {
            AssertMonthDemoData(this.parser.Parse(DemoFiles.MilleniumParser_Family2026_03), new DateTime(2026, 03, 01), new DateTime(2026, 03, 31));
        }

        [Test]
        public void GivenDemoCsv04_WhenParsed_ThenOperationsAreLoaded()
        {
            AssertMonthDemoData(this.parser.Parse(DemoFiles.MilleniumParser_Family2026_04), new DateTime(2026, 04, 01), new DateTime(2026, 04, 30));
        }

        [Test]
        public void GivenDemoCsv05_WhenParsed_ThenOperationsAreLoaded()
        {
            AssertMonthDemoData(this.parser.Parse(DemoFiles.MilleniumParser_Family2026_05), new DateTime(2026, 05, 01), new DateTime(2026, 05, 31));
        }

        [Test]
        public void GivenDemoCsv06_WhenParsed_ThenOperationsAreLoaded()
        {
            AssertMonthDemoData(this.parser.Parse(DemoFiles.MilleniumParser_Family2026_06), new DateTime(2026, 06, 01), new DateTime(2026, 06, 30));
        }

        [Test]
        public void GivenDemoCsv07_WhenParsed_ThenOperationsAreLoaded()
        {
            AssertMonthDemoData(this.parser.Parse(DemoFiles.MilleniumParser_Family2026_07), new DateTime(2026, 07, 01), new DateTime(2026, 07, 31));
        }

        [Test]
        public void GivenDemoCsv08_WhenParsed_ThenOperationsAreLoaded()
        {
            AssertMonthDemoData(this.parser.Parse(DemoFiles.MilleniumParser_Family2026_08), new DateTime(2026, 08, 01), new DateTime(2026, 08, 31));
        }

        private void AssertMonthDemoData(IEnumerable<BankOperation> operations, DateTime expectedFrom, DateTime expectedTo)
        {
            var list = operations.ToList();
            Assert.AreEqual(10, list.Count);
            Assert.IsTrue(list.All(op => op.Cleared));
            Assert.IsTrue(list.All(op => op.BankAccount != null));
            Assert.IsTrue(list.All(op => op.OrderDate >= expectedFrom && op.OrderDate <= expectedTo));
            Assert.IsTrue(list.All(op => !string.IsNullOrWhiteSpace(op.Description)));
            Assert.IsTrue(list.All(op => op.Type != null));
            Assert.IsTrue(list.All(op => op.Amount > 0));
            Assert.AreEqual(2, list.Count(op => op.Type.Name == "PRZELEW PRZYCHODZĄCY"));
            Assert.AreEqual(8, list.Count(op => op.Type.Name == "OBCIĄŻENIE"));
            Assert.IsTrue(list.Any(op => op.CounterParty.Contains("Wynagrodzenie")));
            Assert.IsTrue(list.Any(op => op.Description.Contains("Czynsz")));
        }
    }
}
