using Moq;
using MyBudget.Core.DataContext;
using MyBudget.Model;
using MyBudget.OperationsLoading.BnpParibasXlsx;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;


namespace MyBudget.OperationsLoading.Tests.BnpParibasXlsx
{
    [TestFixture("v1")]
    [TestFixture("v2")]
    [TestFixture("v3")]
    public class BnpParibasXslxParserCrossVersionTests
    {
        private Mock<IRepository<BankAccount, string>> accountRepo;
        private Mock<IRepository<BankOperationType, string>> typeRepo;
        private Mock<IRepository<Card, string>> cardRepo;
        private RepositoryHelper repositoryHelper;
        private BnpParibasXslxParser parser;
        private ParseHelper parseHelper = new ParseHelper();

        private List<BankAccount> mockAccountsCreated = new List<BankAccount>();
        private Stream XlsxFile = null;
        Dictionary<string, Stream> BNPFileVersions = new Dictionary<string, Stream>()
        {
            {"v1", Resources.TestFiles.BNP_TestOperations_v1.ToStream() },
            {"v2", Resources.TestFiles.BNP_TestOperations_v2.ToStream() },
            {"v3", Resources.TestFiles.BNP_TestOperations_v3.ToStream() }
        };

        public BnpParibasXslxParserCrossVersionTests(string version)
        {
            XlsxFile = BNPFileVersions[version];
        }

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

            this.accountRepo.Setup(a => a.Get(It.IsAny<string>())).Returns<string>(
                a => mockAccountsCreated.FirstOrDefault(x => x.Number == a));
            this.accountRepo.Setup(a => a.Add(It.IsAny<BankAccount>())).Callback<BankAccount>(
                a => mockAccountsCreated.Add(a));
        }


        [Test]
        public void GivenBasicCases_WhenParseBnpFormat_Then10OperationsAreReturned_AccountAnd8NewTypesAreAdded()
        {
            //Given                
            var operations = parser.Parse(XlsxFile);

            //Then
            foreach (var newVersion in BNPFileVersions)
            {
                Assert.DoesNotThrow(() => parser.Parse(newVersion.Value),
                    string.Format("Version {0} failed to load", newVersion.Key));
            }         
        }
    }
}
