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
            string pkoBpList = ManifestStreamReaderHelper.ReadEmbeddedResource(
                typeof(PkoBpParserTests).Assembly, 
                "MyBudget.Core.UnitTests.PkoBp1Entry.xml");
            
            //When
            var list = new PkoBpParser().Parse(pkoBpList).ToArray();

            //Then
            Assert.AreEqual(1, list.Count());
        }

        [Test]
        public void GivenPkoBpXmlFileWith1Entry_WhenParse_ThenListOf1EntryReturned()
        {
            //Given
            using(Stream pkoBpList = typeof(PkoBpParserTests).Assembly
                .GetManifestResourceStream("MyBudget.Core.UnitTests.PkoBp1Entry.xml"))
            {
                //When
                var list = new PkoBpParser().Parse(pkoBpList).ToArray();

                //Then
                Assert.AreEqual(1, list.Count());
            }
        }
    }
}
