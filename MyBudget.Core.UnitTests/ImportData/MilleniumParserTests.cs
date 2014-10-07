using Moq;
using MyBudget.Core.DataContext;
using MyBudget.Core.ImportData;
using MyBudget.Core.InMemoryPersistance;
using MyBudget.Core.Model;
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
        [Test]
        public void GivenSampleMilleniumCsv_WhenParse_ThenListOfOperationsReturnedAnd2AccountsCreated()
        {
            BankAccountXmlRepository bankRepo = new BankAccountXmlRepository();
            BankOperationTypeXmlRepository typeRepo = new BankOperationTypeXmlRepository();
            //Given

            string milleniumCsv = ManifestStreamReaderHelper.ReadEmbeddedResource(
                typeof(PkoBpParserTests).Assembly,
                "MyBudget.Core.UnitTests.ImportData.sampleInput.csv");

            //When
            var list = new MilleniumParser(
                new ParseHelper(bankRepo, typeRepo))
                .Parse(milleniumCsv).ToArray();

            //Then
            Assert.AreEqual(7, list.Count());
            Assert.AreEqual(2, bankRepo.GetAll().Count());
            Assert.AreEqual(6, typeRepo.GetAll().Count());
        }
    }
}
