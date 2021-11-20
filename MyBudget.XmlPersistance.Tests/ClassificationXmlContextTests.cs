using MyBudget.Core.DataContext;
using MyBudget.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.XmlPersistance.Tests
{
    [TestFixture]
    public class ClassificationXmlContextTests : XmlContextTestsBase
    {
        [Test]
        public void GivenEmptyContext_WhenClassificationRuleIsAddedAndSave_ThenCanBeLoaded()
        {
            //Given
            var saveContext = GetNewContext();

            //When
            var repo = saveContext.GetRepository<IRepository<ClassificationDefinition>>();
            ClassificationDefinition def = new ClassificationDefinition();
            def.Category = "Cat";
            def.Description = "Desc";
            def.SubCategory = "Sub";
            def.Rules.Add(new ClassificationRule() { SearchedPhrase = "Test1" });
            def.Rules.Add(new ClassificationRule() { SearchedPhrase = "Test2", Account = "Acc1" });
            def.Rules.Add(new ClassificationRule() { SearchedPhrase = "Test3", CounterAccount = "Acc2" });

            repo.Add(def);
            saveContext.SaveChanges();

            //Then
            var loadContext = GetNewContext();
            var repoLoad = saveContext.GetRepository<IRepository<ClassificationDefinition>>();
            var loadedDefinition = repoLoad.GetAll().Single();


            Assert.AreEqual("Cat", loadedDefinition.Category);
            Assert.AreEqual("Desc", loadedDefinition.Description);
            Assert.AreEqual("Sub", loadedDefinition.SubCategory);
            Assert.AreEqual(3, loadedDefinition.Rules.Count);
            Assert.AreEqual(1, loadedDefinition.Rules.Count(a => a.SearchedPhrase == "Test1" && a.Account == null && a.CounterAccount == null));
            Assert.AreEqual(1, loadedDefinition.Rules.Count(a => a.SearchedPhrase == "Test2" && a.Account == "Acc1" && a.CounterAccount == null));
            Assert.AreEqual(1, loadedDefinition.Rules.Count(a => a.SearchedPhrase == "Test3" && a.Account == null && a.CounterAccount == "Acc2"));
        }
    }
}
