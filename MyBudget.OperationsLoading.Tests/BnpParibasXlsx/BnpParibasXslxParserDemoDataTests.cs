using Moq;
using MyBudget.Core.DataContext;
using MyBudget.Model;
using MyBudget.OperationsLoading;
using MyBudget.OperationsLoading.BnpParibasXlsx;
using MyBudget.OperationsLoading.Tests.Resources;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyBudget.OperationsLoading.Tests.BnpParibasXlsx
{
    [TestFixture]
    public class BnpParibasXslxParserDemoDataTests
    {
        private Mock<IRepository<BankAccount, string>> accountRepo;
        private Mock<IRepository<BankOperationType, string>> typeRepo;
        private Mock<IRepository<Card, string>> cardRepo;
        private RepositoryHelper repositoryHelper;
        private BnpParibasXslxParser parser;
        private ParseHelper parseHelper = new ParseHelper();
        private List<BankAccount> mockAccountsCreated = new List<BankAccount>();

        [SetUp]
        public void SetUp()
        {
            this.accountRepo = new Mock<IRepository<BankAccount, string>>();
            this.typeRepo = new Mock<IRepository<BankOperationType, string>>();
            this.cardRepo = new Mock<IRepository<Card, string>>();
            this.repositoryHelper = new RepositoryHelper(accountRepo.Object, typeRepo.Object, cardRepo.Object);
            this.parser = new BnpParibasXslxParser(repositoryHelper,
                new BlikHandler(parseHelper,
                new CardHandler(parseHelper, repositoryHelper,
                new DefaultHandler(parseHelper))));

            this.accountRepo.Setup(a => a.Get(It.IsAny<string>())).Returns<string>(a => mockAccountsCreated.FirstOrDefault(x => x.Number == a));
            this.accountRepo.Setup(a => a.Add(It.IsAny<BankAccount>())).Callback<BankAccount>(a => mockAccountsCreated.Add(a));
        }

        [Test] public void GivenDemoWorkbook03_WhenParsed_ThenOperationsAreLoaded() => AssertDemoMonth(DemoFiles.BNP_TestOperations_2026_03, new DateTime(2026, 03, 01), new DateTime(2026, 03, 31));
        [Test] public void GivenDemoWorkbook04_WhenParsed_ThenOperationsAreLoaded() => AssertDemoMonth(DemoFiles.BNP_TestOperations_2026_04, new DateTime(2026, 04, 01), new DateTime(2026, 04, 30));
        [Test] public void GivenDemoWorkbook05_WhenParsed_ThenOperationsAreLoaded() => AssertDemoMonth(DemoFiles.BNP_TestOperations_2026_05, new DateTime(2026, 05, 01), new DateTime(2026, 05, 31));
        [Test] public void GivenDemoWorkbook06_WhenParsed_ThenOperationsAreLoaded() => AssertDemoMonth(DemoFiles.BNP_TestOperations_2026_06, new DateTime(2026, 06, 01), new DateTime(2026, 06, 30));
        [Test] public void GivenDemoWorkbook07_WhenParsed_ThenOperationsAreLoaded() => AssertDemoMonth(DemoFiles.BNP_TestOperations_2026_07, new DateTime(2026, 07, 01), new DateTime(2026, 07, 31));
        [Test] public void GivenDemoWorkbook08_WhenParsed_ThenOperationsAreLoaded() => AssertDemoMonth(DemoFiles.BNP_TestOperations_2026_08, new DateTime(2026, 08, 01), new DateTime(2026, 08, 31));

        private void AssertDemoMonth(byte[] workbook, DateTime expectedFrom, DateTime expectedTo)
        {
            var operations = parser.Parse(workbook.ToStream()).ToList();

            Assert.AreEqual(10, operations.Count);
            Assert.IsTrue(operations.All(op => op.Cleared));
            Assert.IsTrue(operations.All(op => op.BankAccount != null));
            Assert.IsTrue(operations.All(op => op.OrderDate >= expectedFrom && op.OrderDate <= expectedTo));
            Assert.IsTrue(operations.All(op => op.ExecutionDate >= expectedFrom && op.ExecutionDate <= expectedTo));
            Assert.IsTrue(operations.All(op => !string.IsNullOrWhiteSpace(op.Description)));
            Assert.IsTrue(operations.All(op => op.Type != null));
            Assert.AreEqual(3, operations.Select(op => op.Type.Name).Distinct().Count());
            Assert.AreEqual(1, operations.Count(op => op.Amount > 0));
            Assert.AreEqual(9, operations.Count(op => op.Amount < 0));
            Assert.AreEqual(1, mockAccountsCreated.Count);
            Assert.AreEqual(TestBankData.BNPParibas_TestAccount1.Compact(), mockAccountsCreated[0].Number);
        }
    }
}
