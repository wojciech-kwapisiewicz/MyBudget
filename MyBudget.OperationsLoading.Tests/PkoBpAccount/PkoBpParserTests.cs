using Moq;
using MyBudget.Core.DataContext;
using MyBudget.Model;
using MyBudget.OperationsLoading;
using MyBudget.OperationsLoading.PkoBpAccount;
using MyBudget.OperationsLoading.Tests.Resources;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MyBudget.Core.UnitTests.ImportData
{
    [TestFixture]
    public class PkoBpParserTests
    {
        Mock<IRepository<BankAccount, string>> accountRepo;
        Mock<IRepository<BankOperationType, string>> typeRepo;
        PkoBpParser parser;


        [SetUp]
        public void Init()
        {
            this.accountRepo = new Mock<IRepository<BankAccount, string>>();
            this.typeRepo = new Mock<IRepository<BankOperationType, string>>();
            this.parser = new PkoBpParser(new ParseHelper(), new RepositoryHelper(accountRepo.Object, typeRepo.Object));
        }

        [Test]
        public void GivenEmptyRepositoriesAndPkoBpXmlTextWith1Entry_WhenParse_ThenBankAccountAddedAndListOf1EntryReturned()
        {
            //Given            
            string pkoBpList = TestFiles.PkoBpParser_1Entry;
            
            //When
            var list = this.parser.Parse(pkoBpList).ToArray();

            //Then
            Assert.AreEqual(1, list.Count());
            Assert.IsTrue(list.Any(a => a.Title == "SomeTitle"));
            accountRepo.Verify(a => a.Add(It.IsAny<BankAccount>()));
            typeRepo.Verify(a => a.Add(It.IsAny<BankOperationType>()));
        }

        [Test]
        public void GivenEmptyRepositoriesAndPkoBpXmlFileWith1Entry_WhenParse_ThenAccountAndTypeAddedAndListOf1EntryReturned()
        {
            //Given
            using (Stream pkoBpList = ToStream(TestFiles.PkoBpParser_1Entry))
            {
                //When
                var list = this.parser.Parse(pkoBpList).ToArray();

                //Then
                Assert.AreEqual(1, list.Count());
                accountRepo.Verify(a => a.Add(It.IsAny<BankAccount>()));
                typeRepo.Verify(a => a.Add(It.IsAny<BankOperationType>()));
            }
        }

        public Stream ToStream(string text)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(text);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        [Test]
        public void GivenWithdrawalAndCardOperation_WhenParse_TitleIsTakenFromLocation()
        {
            //Given            
            string pkoBpList = TestFiles.PkoBpParser_ZerosInTitle;

            //When
            var list = this.parser.Parse(pkoBpList).ToArray();

            //
            Assert.AreEqual(2, list.Count());
            Assert.IsTrue(list.Any(a => a.Title == "U AAA 222 Miasto: CityOfSth Kraj: POLSKA"));
            Assert.IsTrue(list.Any(a => a.Title == "Supermarket ABC Miasto: CityOfSth Kraj: POLSKA"));
        }
    }
}
