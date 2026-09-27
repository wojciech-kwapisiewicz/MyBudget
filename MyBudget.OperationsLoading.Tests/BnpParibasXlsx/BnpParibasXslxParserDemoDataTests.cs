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

        [Test] public void GivenDemoWorkbook03_WhenParsed_ThenOperationsAreLoaded() => AssertDemoMonth(DemoFiles.BNP_TestOperations_2026_03, new[]
            {
                new ExpectedOperation(6800.00M, "Przelew wynagrodzenia - Anna Nowak", "PRZELEW PRZYCHODZĄCY", new DateTime(2026, 03, 01)),
                new ExpectedOperation(-2650.00M, "Czynsz za mieszkanie", "PRZELEW WYKONANY", new DateTime(2026, 03, 02)),
                new ExpectedOperation(-487.80M, "Zakupy spożywcze dla rodziny", "PŁATNOŚĆ KARTĄ", new DateTime(2026, 03, 03)),
                new ExpectedOperation(-782.20M, "Czesne za przedszkole", "PRZELEW WYKONANY", new DateTime(2026, 03, 04)),
                new ExpectedOperation(-164.90M, "Artykuły dla dzieci", "PŁATNOŚĆ KARTĄ", new DateTime(2026, 03, 05)),
                new ExpectedOperation(-133.50M, "Chemia domowa i kosmetyki", "PŁATNOŚĆ KARTĄ", new DateTime(2026, 03, 06)),
                new ExpectedOperation(-99.00M, "Leki dla rodziny", "PŁATNOŚĆ KARTĄ", new DateTime(2026, 03, 07)),
                new ExpectedOperation(-218.50M, "Ubrania dziecięce", "PŁATNOŚĆ KARTĄ", new DateTime(2026, 03, 08)),
                new ExpectedOperation(-75.09M, "Książki i materiały szkolne", "PŁATNOŚĆ KARTĄ", new DateTime(2026, 03, 09)),
                new ExpectedOperation(-360.50M, "Zakupy spożywcze - weekend", "PŁATNOŚĆ KARTĄ", new DateTime(2026, 03, 10))
            });

        [Test] public void GivenDemoWorkbook04_WhenParsed_ThenOperationsAreLoaded() => AssertDemoMonth(DemoFiles.BNP_TestOperations_2026_04, new[]
            {
                new ExpectedOperation(6900.00M, "Przelew wynagrodzenia - Anna Nowak", "PRZELEW PRZYCHODZĄCY", new DateTime(2026, 04, 01)),
                new ExpectedOperation(-2661.20M, "Czynsz za mieszkanie", "PRZELEW WYKONANY", new DateTime(2026, 04, 02)),
                new ExpectedOperation(-499.00M, "Zakupy spożywcze dla rodziny", "PŁATNOŚĆ KARTĄ", new DateTime(2026, 04, 03)),
                new ExpectedOperation(-793.40M, "Czesne za przedszkole", "PRZELEW WYKONANY", new DateTime(2026, 04, 04)),
                new ExpectedOperation(-176.10M, "Artykuły dla dzieci", "PŁATNOŚĆ KARTĄ", new DateTime(2026, 04, 05)),
                new ExpectedOperation(-144.70M, "Chemia domowa i kosmetyki", "PŁATNOŚĆ KARTĄ", new DateTime(2026, 04, 06)),
                new ExpectedOperation(-110.20M, "Leki dla rodziny", "PŁATNOŚĆ KARTĄ", new DateTime(2026, 04, 07)),
                new ExpectedOperation(-229.70M, "Ubrania dziecięce", "PŁATNOŚĆ KARTĄ", new DateTime(2026, 04, 08)),
                new ExpectedOperation(-86.29M, "Książki i materiały szkolne", "PŁATNOŚĆ KARTĄ", new DateTime(2026, 04, 09)),
                new ExpectedOperation(-371.70M, "Zakupy spożywcze - weekend", "PŁATNOŚĆ KARTĄ", new DateTime(2026, 04, 10))
            });

        [Test] public void GivenDemoWorkbook05_WhenParsed_ThenOperationsAreLoaded() => AssertDemoMonth(DemoFiles.BNP_TestOperations_2026_05, new[]
            {
                new ExpectedOperation(7000.00M, "Przelew wynagrodzenia - Anna Nowak", "PRZELEW PRZYCHODZĄCY", new DateTime(2026, 05, 01)),
                new ExpectedOperation(-2641.50M, "Czynsz za mieszkanie", "PRZELEW WYKONANY", new DateTime(2026, 05, 02)),
                new ExpectedOperation(-479.30M, "Zakupy spożywcze dla rodziny", "PŁATNOŚĆ KARTĄ", new DateTime(2026, 05, 03)),
                new ExpectedOperation(-773.70M, "Czesne za przedszkole", "PRZELEW WYKONANY", new DateTime(2026, 05, 04)),
                new ExpectedOperation(-156.40M, "Artykuły dla dzieci", "PŁATNOŚĆ KARTĄ", new DateTime(2026, 05, 05)),
                new ExpectedOperation(-125.00M, "Chemia domowa i kosmetyki", "PŁATNOŚĆ KARTĄ", new DateTime(2026, 05, 06)),
                new ExpectedOperation(-90.50M, "Leki dla rodziny", "PŁATNOŚĆ KARTĄ", new DateTime(2026, 05, 07)),
                new ExpectedOperation(-210.00M, "Ubrania dziecięce", "PŁATNOŚĆ KARTĄ", new DateTime(2026, 05, 08)),
                new ExpectedOperation(-66.59M, "Książki i materiały szkolne", "PŁATNOŚĆ KARTĄ", new DateTime(2026, 05, 09)),
                new ExpectedOperation(-352.00M, "Zakupy spożywcze - weekend", "PŁATNOŚĆ KARTĄ", new DateTime(2026, 05, 10))
            });

        [Test] public void GivenDemoWorkbook06_WhenParsed_ThenOperationsAreLoaded() => AssertDemoMonth(DemoFiles.BNP_TestOperations_2026_06, new[]
            {
                new ExpectedOperation(7100.00M, "Przelew wynagrodzenia - Anna Nowak", "PRZELEW PRZYCHODZĄCY", new DateTime(2026, 06, 01)),
                new ExpectedOperation(-2666.40M, "Czynsz za mieszkanie", "PRZELEW WYKONANY", new DateTime(2026, 06, 02)),
                new ExpectedOperation(-504.20M, "Zakupy spożywcze dla rodziny", "PŁATNOŚĆ KARTĄ", new DateTime(2026, 06, 03)),
                new ExpectedOperation(-798.60M, "Czesne za przedszkole", "PRZELEW WYKONANY", new DateTime(2026, 06, 04)),
                new ExpectedOperation(-181.30M, "Artykuły dla dzieci", "PŁATNOŚĆ KARTĄ", new DateTime(2026, 06, 05)),
                new ExpectedOperation(-149.90M, "Chemia domowa i kosmetyki", "PŁATNOŚĆ KARTĄ", new DateTime(2026, 06, 06)),
                new ExpectedOperation(-115.40M, "Leki dla rodziny", "PŁATNOŚĆ KARTĄ", new DateTime(2026, 06, 07)),
                new ExpectedOperation(-234.90M, "Ubrania dziecięce", "PŁATNOŚĆ KARTĄ", new DateTime(2026, 06, 08)),
                new ExpectedOperation(-91.49M, "Książki i materiały szkolne", "PŁATNOŚĆ KARTĄ", new DateTime(2026, 06, 09)),
                new ExpectedOperation(-376.90M, "Zakupy spożywcze - weekend", "PŁATNOŚĆ KARTĄ", new DateTime(2026, 06, 10))
            });

        [Test] public void GivenDemoWorkbook07_WhenParsed_ThenOperationsAreLoaded() => AssertDemoMonth(DemoFiles.BNP_TestOperations_2026_07, new[]
            {
                new ExpectedOperation(7200.00M, "Przelew wynagrodzenia - Anna Nowak", "PRZELEW PRZYCHODZĄCY", new DateTime(2026, 07, 01)),
                new ExpectedOperation(-2644.30M, "Czynsz za mieszkanie", "PRZELEW WYKONANY", new DateTime(2026, 07, 02)),
                new ExpectedOperation(-482.10M, "Zakupy spożywcze dla rodziny", "PŁATNOŚĆ KARTĄ", new DateTime(2026, 07, 03)),
                new ExpectedOperation(-776.50M, "Czesne za przedszkole", "PRZELEW WYKONANY", new DateTime(2026, 07, 04)),
                new ExpectedOperation(-159.20M, "Artykuły dla dzieci", "PŁATNOŚĆ KARTĄ", new DateTime(2026, 07, 05)),
                new ExpectedOperation(-127.80M, "Chemia domowa i kosmetyki", "PŁATNOŚĆ KARTĄ", new DateTime(2026, 07, 06)),
                new ExpectedOperation(-93.30M, "Leki dla rodziny", "PŁATNOŚĆ KARTĄ", new DateTime(2026, 07, 07)),
                new ExpectedOperation(-212.80M, "Ubrania dziecięce", "PŁATNOŚĆ KARTĄ", new DateTime(2026, 07, 08)),
                new ExpectedOperation(-69.39M, "Książki i materiały szkolne", "PŁATNOŚĆ KARTĄ", new DateTime(2026, 07, 09)),
                new ExpectedOperation(-354.80M, "Zakupy spożywcze - weekend", "PŁATNOŚĆ KARTĄ", new DateTime(2026, 07, 10))
            });

        [Test] public void GivenDemoWorkbook08_WhenParsed_ThenOperationsAreLoaded() => AssertDemoMonth(DemoFiles.BNP_TestOperations_2026_08, new[]
            {
                new ExpectedOperation(7300.00M, "Przelew wynagrodzenia - Anna Nowak", "PRZELEW PRZYCHODZĄCY", new DateTime(2026, 08, 01)),
                new ExpectedOperation(-2672.10M, "Czynsz za mieszkanie", "PRZELEW WYKONANY", new DateTime(2026, 08, 02)),
                new ExpectedOperation(-509.90M, "Zakupy spożywcze dla rodziny", "PŁATNOŚĆ KARTĄ", new DateTime(2026, 08, 03)),
                new ExpectedOperation(-804.30M, "Czesne za przedszkole", "PRZELEW WYKONANY", new DateTime(2026, 08, 04)),
                new ExpectedOperation(-187.00M, "Artykuły dla dzieci", "PŁATNOŚĆ KARTĄ", new DateTime(2026, 08, 05)),
                new ExpectedOperation(-155.60M, "Chemia domowa i kosmetyki", "PŁATNOŚĆ KARTĄ", new DateTime(2026, 08, 06)),
                new ExpectedOperation(-121.10M, "Leki dla rodziny", "PŁATNOŚĆ KARTĄ", new DateTime(2026, 08, 07)),
                new ExpectedOperation(-240.60M, "Ubrania dziecięce", "PŁATNOŚĆ KARTĄ", new DateTime(2026, 08, 08)),
                new ExpectedOperation(-97.19M, "Książki i materiały szkolne", "PŁATNOŚĆ KARTĄ", new DateTime(2026, 08, 09)),
                new ExpectedOperation(-382.60M, "Zakupy spożywcze - weekend", "PŁATNOŚĆ KARTĄ", new DateTime(2026, 08, 10))
            });

        private void AssertDemoMonth(byte[] workbook, ExpectedOperation[] expectedOperations)
        {
            var operations = parser.Parse(workbook.ToStream()).ToList();

            Assert.AreEqual(expectedOperations.Length, operations.Count);
            for (var index = 0; index < expectedOperations.Length; index++)
            {
                AssertOperation(operations[index], expectedOperations[index]);
            }

            Assert.AreEqual(1, mockAccountsCreated.Count);
            Assert.AreEqual(TestBankData.BNPParibas_TestAccount1.Compact(), mockAccountsCreated[0].Number);
        }

        private static void AssertOperation(BankOperation actual, ExpectedOperation expected)
        {
            Assert.AreEqual(expected.Amount, actual.Amount);
            Assert.AreEqual(expected.Title, actual.Title);
            Assert.AreEqual(expected.Type, actual.Type.Name);
            Assert.AreEqual(expected.OrderDate, actual.OrderDate);
            Assert.AreEqual(expected.Amount > 0, actual.Amount > 0);
            Assert.IsTrue(actual.Cleared);
        }

        private class ExpectedOperation
        {
            public ExpectedOperation(decimal amount, string title, string type, DateTime orderDate)
            {
                Amount = amount;
                Title = title;
                Type = type;
                OrderDate = orderDate;
            }

            public decimal Amount { get; private set; }
            public string Title { get; private set; }
            public string Type { get; private set; }
            public DateTime OrderDate { get; private set; }
        }
    }
}
