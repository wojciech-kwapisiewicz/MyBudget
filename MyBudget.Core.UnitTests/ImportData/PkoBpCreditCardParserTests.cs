using MyBudget.Core.ImportData;
using MyBudget.Core.InMemoryPersistance;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Core.UnitTests.ImportData
{
    [TestFixture]
    public class PkoBpCreditCardParserTests
    {
        [Test]
        public void GivenSampleCreditCardText_WhenParse_ThenListOfOperationsReturnedAndAccountCreated()
        {
            BankAccountXmlRepository bankRepo = new BankAccountXmlRepository();
            BankOperationTypeXmlRepository typeRepo = new BankOperationTypeXmlRepository();
            //Given

            string creditCardText = TestFiles.PkoBpCreditCardParser_Sample;

            //When
            var list = new PkoBpCreditCardUnclearedParser(
                new ParseHelper(bankRepo, typeRepo),
                new CreditCardTextParsing(),
                new CreditCardUnclearedTextParsing())
                .Parse(creditCardText).ToArray();

            //Then
            Assert.AreEqual(3, list.Count());
            Assert.AreEqual(1, bankRepo.GetAll().Count());
            Assert.AreEqual(2, typeRepo.GetAll().Count());
        }
    }
}
