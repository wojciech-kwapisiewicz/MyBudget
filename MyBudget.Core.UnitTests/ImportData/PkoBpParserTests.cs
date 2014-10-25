using Moq;
using MyBudget.Core.DataContext;
using MyBudget.Core.ImportData;
using MyBudget.Core.Model;
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
        [Test]
        public void GivenEmptyRepositoriesAndPkoBpXmlTextWith1Entry_WhenParse_ThenBankAccountAddedAndListOf1EntryReturned()
        {
            //Given
            Mock<IRepository<BankAccount, string>> accountRepo = new Mock<IRepository<BankAccount, string>>();
            Mock<IRepository<BankOperationType, string>> typeRepo = new Mock<IRepository<BankOperationType, string>>();
            string pkoBpList = TestFiles.PkoBpParser_1Entry;
            
            //When
            var list = new PkoBpParser(
                new ParseHelper(accountRepo.Object, typeRepo.Object))
                .Parse(pkoBpList).ToArray();

            //Then
            Assert.AreEqual(1, list.Count());
            accountRepo.Verify(a => a.Add(It.IsAny<BankAccount>()));
            typeRepo.Verify(a => a.Add(It.IsAny<BankOperationType>()));
        }

        [Test]
        public void GivenEmptyRepositoriesAndPkoBpXmlFileWith1Entry_WhenParse_ThenAccountAndTypeAddedAndListOf1EntryReturned()
        {
            Mock<IRepository<BankAccount, string>> accountRepo = new Mock<IRepository<BankAccount, string>>();
            Mock<IRepository<BankOperationType, string>> typeRepo = new Mock<IRepository<BankOperationType, string>>();

            //Given
            using (Stream pkoBpList = ToStream(TestFiles.PkoBpParser_1Entry))
            {
                //When
                var list = new PkoBpParser(
                    new ParseHelper(accountRepo.Object, typeRepo.Object))
                    .Parse(pkoBpList).ToArray();

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
    }
}
