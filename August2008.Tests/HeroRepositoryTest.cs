using System;
using System.Linq;
using August2008.Data;
using August2008.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace August2008.Tests
{
    [TestClass]
    public class HeroRepositoryTest
    {
        [TestMethod]
        public void HeroRepository_Can_GetHeros() 
        {
            var repository = new HeroRepository(null);
            var heros = repository.SearchHeros(new HeroSearchCriteria  { 
                PageNo = 1,
                PageSize = 10,
                LanguageId = 1});
            Assert.IsTrue(heros.Result.Count() > 0);
        }
        [TestMethod]
        public void HeroRepository_Can_GetHeroAlphabet()
        {
            var repository = new HeroRepository(null);
            var alphabet = repository.GetAlphabet(1);
            Assert.IsTrue(alphabet.Count() > 0);
        }
        [TestMethod]
        public void HeroRepository_Can_GetRandomHero()
        {
            var repository = new HeroRepository(null);
            var hero = repository.GetRandomHero(1);
            Assert.IsTrue(hero.HeroId > 0);
        }
    }
}
