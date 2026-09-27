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
            AssertDemoMonth(this.parser.Parse(DemoFiles.MilleniumParser_Family2026_03), new[]
            {
                new ExpectedOperation(7200.00M, "Przelew wynagrodzenia - Piotr Nowak", "PRZELEW PRZYCHODZ\u0104CY", new DateTime(2026, 03, 01)),
                new ExpectedOperation(249.00M, "PGE - faktura za pr\u0105d", "OBCI\u0104\u017bENIE", new DateTime(2026, 03, 02)),
                new ExpectedOperation(178.60M, "Gaz - op\u0142ata miesi\u0119czna", "OBCI\u0104\u017bENIE", new DateTime(2026, 03, 03)),
                new ExpectedOperation(151.09M, "Internet i telefon domowy", "OBCI\u0104\u017bENIE", new DateTime(2026, 03, 04)),
                new ExpectedOperation(286.90M, "Tankowanie samochodu", "OBCI\u0104\u017bENIE", new DateTime(2026, 03, 05)),
                new ExpectedOperation(125.00M, "Ubezpieczenie samochodu - rata", "OBCI\u0104\u017bENIE", new DateTime(2026, 03, 06)),
                new ExpectedOperation(44.10M, "Abonament Netflix", "OBCI\u0104\u017bENIE", new DateTime(2026, 03, 07)),
                new ExpectedOperation(191.20M, "Wyposa\u017cenie mieszkania", "OBCI\u0104\u017bENIE", new DateTime(2026, 03, 08)),
                new ExpectedOperation(24.99M, "Przesy\u0142ka dla rodziny", "OBCI\u0104\u017bENIE", new DateTime(2026, 03, 09)),
                new ExpectedOperation(157.10M, "Bilety rodzinne", "OBCI\u0104\u017bENIE", new DateTime(2026, 03, 10))
            });
        }

        [Test]
        public void GivenDemoCsv04_WhenParsed_ThenOperationsAreLoaded()
        {
            AssertDemoMonth(this.parser.Parse(DemoFiles.MilleniumParser_Family2026_04), new[]
            {
                new ExpectedOperation(7300.00M, "Przelew wynagrodzenia - Piotr Nowak", "PRZELEW PRZYCHODZ\u0104CY", new DateTime(2026, 04, 01)),
                new ExpectedOperation(260.20M, "PGE - faktura za pr\u0105d", "OBCI\u0104\u017bENIE", new DateTime(2026, 04, 02)),
                new ExpectedOperation(189.80M, "Gaz - op\u0142ata miesi\u0119czna", "OBCI\u0104\u017bENIE", new DateTime(2026, 04, 03)),
                new ExpectedOperation(162.29M, "Internet i telefon domowy", "OBCI\u0104\u017bENIE", new DateTime(2026, 04, 04)),
                new ExpectedOperation(298.10M, "Tankowanie samochodu", "OBCI\u0104\u017bENIE", new DateTime(2026, 04, 05)),
                new ExpectedOperation(136.20M, "Ubezpieczenie samochodu - rata", "OBCI\u0104\u017bENIE", new DateTime(2026, 04, 06)),
                new ExpectedOperation(55.30M, "Abonament Netflix", "OBCI\u0104\u017bENIE", new DateTime(2026, 04, 07)),
                new ExpectedOperation(202.40M, "Wyposa\u017cenie mieszkania", "OBCI\u0104\u017bENIE", new DateTime(2026, 04, 08)),
                new ExpectedOperation(36.19M, "Przesy\u0142ka dla rodziny", "OBCI\u0104\u017bENIE", new DateTime(2026, 04, 09)),
                new ExpectedOperation(168.30M, "Bilety rodzinne", "OBCI\u0104\u017bENIE", new DateTime(2026, 04, 10))
            });
        }

        [Test]
        public void GivenDemoCsv05_WhenParsed_ThenOperationsAreLoaded()
        {
            AssertDemoMonth(this.parser.Parse(DemoFiles.MilleniumParser_Family2026_05), new[]
            {
                new ExpectedOperation(7400.00M, "Przelew wynagrodzenia - Piotr Nowak", "PRZELEW PRZYCHODZ\u0104CY", new DateTime(2026, 05, 01)),
                new ExpectedOperation(240.50M, "PGE - faktura za pr\u0105d", "OBCI\u0104\u017bENIE", new DateTime(2026, 05, 02)),
                new ExpectedOperation(170.10M, "Gaz - op\u0142ata miesi\u0119czna", "OBCI\u0104\u017bENIE", new DateTime(2026, 05, 03)),
                new ExpectedOperation(142.59M, "Internet i telefon domowy", "OBCI\u0104\u017bENIE", new DateTime(2026, 05, 04)),
                new ExpectedOperation(278.40M, "Tankowanie samochodu", "OBCI\u0104\u017bENIE", new DateTime(2026, 05, 05)),
                new ExpectedOperation(116.50M, "Ubezpieczenie samochodu - rata", "OBCI\u0104\u017bENIE", new DateTime(2026, 05, 06)),
                new ExpectedOperation(35.60M, "Abonament Netflix", "OBCI\u0104\u017bENIE", new DateTime(2026, 05, 07)),
                new ExpectedOperation(182.70M, "Wyposa\u017cenie mieszkania", "OBCI\u0104\u017bENIE", new DateTime(2026, 05, 08)),
                new ExpectedOperation(16.49M, "Przesy\u0142ka dla rodziny", "OBCI\u0104\u017bENIE", new DateTime(2026, 05, 09)),
                new ExpectedOperation(148.60M, "Bilety rodzinne", "OBCI\u0104\u017bENIE", new DateTime(2026, 05, 10))
            });
        }

        [Test]
        public void GivenDemoCsv06_WhenParsed_ThenOperationsAreLoaded()
        {
            AssertDemoMonth(this.parser.Parse(DemoFiles.MilleniumParser_Family2026_06), new[]
            {
                new ExpectedOperation(7500.00M, "Przelew wynagrodzenia - Piotr Nowak", "PRZELEW PRZYCHODZ\u0104CY", new DateTime(2026, 06, 01)),
                new ExpectedOperation(265.40M, "PGE - faktura za pr\u0105d", "OBCI\u0104\u017bENIE", new DateTime(2026, 06, 02)),
                new ExpectedOperation(195.00M, "Gaz - op\u0142ata miesi\u0119czna", "OBCI\u0104\u017bENIE", new DateTime(2026, 06, 03)),
                new ExpectedOperation(167.49M, "Internet i telefon domowy", "OBCI\u0104\u017bENIE", new DateTime(2026, 06, 04)),
                new ExpectedOperation(303.30M, "Tankowanie samochodu", "OBCI\u0104\u017bENIE", new DateTime(2026, 06, 05)),
                new ExpectedOperation(141.40M, "Ubezpieczenie samochodu - rata", "OBCI\u0104\u017bENIE", new DateTime(2026, 06, 06)),
                new ExpectedOperation(60.50M, "Abonament Netflix", "OBCI\u0104\u017bENIE", new DateTime(2026, 06, 07)),
                new ExpectedOperation(207.60M, "Wyposa\u017cenie mieszkania", "OBCI\u0104\u017bENIE", new DateTime(2026, 06, 08)),
                new ExpectedOperation(41.39M, "Przesy\u0142ka dla rodziny", "OBCI\u0104\u017bENIE", new DateTime(2026, 06, 09)),
                new ExpectedOperation(173.50M, "Bilety rodzinne", "OBCI\u0104\u017bENIE", new DateTime(2026, 06, 10))
            });
        }

        [Test]
        public void GivenDemoCsv07_WhenParsed_ThenOperationsAreLoaded()
        {
            AssertDemoMonth(this.parser.Parse(DemoFiles.MilleniumParser_Family2026_07), new[]
            {
                new ExpectedOperation(7600.00M, "Przelew wynagrodzenia - Piotr Nowak", "PRZELEW PRZYCHODZ\u0104CY", new DateTime(2026, 07, 01)),
                new ExpectedOperation(243.30M, "PGE - faktura za pr\u0105d", "OBCI\u0104\u017bENIE", new DateTime(2026, 07, 02)),
                new ExpectedOperation(172.90M, "Gaz - op\u0142ata miesi\u0119czna", "OBCI\u0104\u017bENIE", new DateTime(2026, 07, 03)),
                new ExpectedOperation(145.39M, "Internet i telefon domowy", "OBCI\u0104\u017bENIE", new DateTime(2026, 07, 04)),
                new ExpectedOperation(281.20M, "Tankowanie samochodu", "OBCI\u0104\u017bENIE", new DateTime(2026, 07, 05)),
                new ExpectedOperation(119.30M, "Ubezpieczenie samochodu - rata", "OBCI\u0104\u017bENIE", new DateTime(2026, 07, 06)),
                new ExpectedOperation(38.40M, "Abonament Netflix", "OBCI\u0104\u017bENIE", new DateTime(2026, 07, 07)),
                new ExpectedOperation(185.50M, "Wyposa\u017cenie mieszkania", "OBCI\u0104\u017bENIE", new DateTime(2026, 07, 08)),
                new ExpectedOperation(19.29M, "Przesy\u0142ka dla rodziny", "OBCI\u0104\u017bENIE", new DateTime(2026, 07, 09)),
                new ExpectedOperation(151.40M, "Bilety rodzinne", "OBCI\u0104\u017bENIE", new DateTime(2026, 07, 10))
            });
        }

        [Test]
        public void GivenDemoCsv08_WhenParsed_ThenOperationsAreLoaded()
        {
            AssertDemoMonth(this.parser.Parse(DemoFiles.MilleniumParser_Family2026_08), new[]
            {
                new ExpectedOperation(7700.00M, "Przelew wynagrodzenia - Piotr Nowak", "PRZELEW PRZYCHODZ\u0104CY", new DateTime(2026, 08, 01)),
                new ExpectedOperation(271.10M, "PGE - faktura za pr\u0105d", "OBCI\u0104\u017bENIE", new DateTime(2026, 08, 02)),
                new ExpectedOperation(200.70M, "Gaz - op\u0142ata miesi\u0119czna", "OBCI\u0104\u017bENIE", new DateTime(2026, 08, 03)),
                new ExpectedOperation(173.19M, "Internet i telefon domowy", "OBCI\u0104\u017bENIE", new DateTime(2026, 08, 04)),
                new ExpectedOperation(309.00M, "Tankowanie samochodu", "OBCI\u0104\u017bENIE", new DateTime(2026, 08, 05)),
                new ExpectedOperation(147.10M, "Ubezpieczenie samochodu - rata", "OBCI\u0104\u017bENIE", new DateTime(2026, 08, 06)),
                new ExpectedOperation(66.20M, "Abonament Netflix", "OBCI\u0104\u017bENIE", new DateTime(2026, 08, 07)),
                new ExpectedOperation(213.30M, "Wyposa\u017cenie mieszkania", "OBCI\u0104\u017bENIE", new DateTime(2026, 08, 08)),
                new ExpectedOperation(47.09M, "Przesy\u0142ka dla rodziny", "OBCI\u0104\u017bENIE", new DateTime(2026, 08, 09)),
                new ExpectedOperation(179.20M, "Bilety rodzinne", "OBCI\u0104\u017bENIE", new DateTime(2026, 08, 10))
            });
        }

        private void AssertDemoMonth(IEnumerable<BankOperation> operations, ExpectedOperation[] expectedOperations)
        {
            var list = operations.ToList();

            Assert.AreEqual(expectedOperations.Length, list.Count);
            for (var index = 0; index < expectedOperations.Length; index++)
            {
                AssertOperation(list[index], expectedOperations[index]);
            }
        }

        private static void AssertOperation(BankOperation actual, ExpectedOperation expected)
        {
            Assert.AreEqual(expected.Amount, actual.Amount);
            Assert.AreEqual(expected.Title, actual.Title);
            Assert.AreEqual(expected.Type, actual.Type.Name);
            Assert.AreEqual(expected.OrderDate, actual.OrderDate);
            Assert.AreEqual(expected.Type == "PRZELEW PRZYCHODZ\u0104CY", actual.Type.Name == "PRZELEW PRZYCHODZ\u0104CY");
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
