using Moq;
using MyBudget.Core.DataContext;
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

namespace MyBudget.Core.UnitTests
{
    [TestFixture]
    public class PkoBpParserTests
    {
        [Test]
        public void GivenPkoBpXmlTextWith1Entry_WhenParse_ThenListOf1EntryReturned()
        {
            //Given
            BankAccount ba = new BankAccount();
            Mock<IRepository<BankAccount, string>> accountRepo = new Mock<IRepository<BankAccount, string>>();
            accountRepo
                .Setup(a => a.Get(It.IsAny<string>()))
                .Returns(ba);

            BankOperationType type = new BankOperationType();
            Mock<IRepository<BankOperationType, string>> typeRepo = new Mock<IRepository<BankOperationType, string>>();
            typeRepo
                .Setup(a=>a.Get(It.IsAny<string>()))
                .Returns(type);

            string pkoBpList = ManifestStreamReaderHelper.ReadEmbeddedResource(
                typeof(PkoBpParserTests).Assembly, 
                "MyBudget.Core.UnitTests.PkoBp1Entry.xml");
            
            //When
            var list = new PkoBpParser(accountRepo.Object, typeRepo.Object)
                .Parse(pkoBpList).ToArray();

            //Then
            Assert.AreEqual(1, list.Count());
            Assert.AreEqual(ba, list.First().BankAccount);
            Assert.AreEqual(type, list.First().Type);
        }

        [Test]
        public void GivenPkoBpXmlFileWith1Entry_WhenParse_ThenListOf1EntryReturned()
        {
            BankAccount ba = new BankAccount();
            Mock<IRepository<BankAccount, string>> accountRepo = new Mock<IRepository<BankAccount, string>>();
            accountRepo
                .Setup(a => a.Get(It.IsAny<string>()))
                .Returns(ba);

            BankOperationType type = new BankOperationType();
            Mock<IRepository<BankOperationType, string>> typeRepo = new Mock<IRepository<BankOperationType, string>>();
            typeRepo
                .Setup(a => a.Get(It.IsAny<string>()))
                .Returns(type);

            //Given
            using(Stream pkoBpList = typeof(PkoBpParserTests).Assembly
                .GetManifestResourceStream("MyBudget.Core.UnitTests.PkoBp1Entry.xml"))
            {
                //When
                var list = new PkoBpParser(accountRepo.Object, typeRepo.Object)
                    .Parse(pkoBpList).ToArray();

                //Then
                Assert.AreEqual(1, list.Count());
                Assert.AreEqual(ba, list.First().BankAccount);
                Assert.AreEqual(type, list.First().Type);
            }
        }
    }
}
