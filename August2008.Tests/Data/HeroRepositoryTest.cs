using System;
using System.Linq;
using August2008.Data;
using August2008.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace August2008.Tests.Data
{
    [TestClass]
    public class HeroRepositoryTest
    {
        [TestMethod]
        public void HeroRepository_Can_Get_Heros()
        {
            var repository = new HeroRepository();
            var heros = repository.GetHeros(new HeroSearchCriteria  { 
                PageNo = 1,
                PageSize = 10,
                LanguageId = 1});
            Assert.IsTrue(heros.Result.Count() > 0);
        }
        [TestMethod]
        public void HeroRepository_Can_Get_HeroAlphabet()
        {
            var repository = new HeroRepository();
            var alphabet = repository.GetAlphabet(1);
            Assert.IsTrue(alphabet.Count() > 0);
        }
    }
}
