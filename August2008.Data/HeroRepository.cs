using System;
using System.Collections.Generic;
using System.Data;
using August2008.Common;
using August2008.Common.Interfaces;
using August2008.Model;

namespace August2008.Data
{
    public sealed class HeroRepository : IHeroRepository
    {
        public Hero GetHero(int heroId, int languageId)
        {
            using (var db = new DataAccess())
            {
                db.CreateStoredProcCommand("dbo.GetHeroDetails");
                db.AddInParameter("@HeroId", DbType.Int32, heroId);
                db.AddInParameter("@LanguageId", DbType.Int32, languageId);

                var hero = new Hero();
                try
                {
                    db.ReadInto(hero);
                }
                catch (Exception)
                {
                    throw;
                }
                return hero;
            }
        }
        public int CreateHero(Hero hero)
        {
            return 0;
        }
        public List<MilitaryRank> GetMilitaryRanks(int languageId)
        {
            using (var db = new DataAccess())
            {
                db.CreateStoredProcCommand("dbo.GetMilitaryRanks");
                db.AddInParameter("@LanguageId", DbType.Int32, languageId);

                var ranks = new List<MilitaryRank>();
                try
                {
                    db.ReadInto(ranks);
                }
                catch (Exception)
                {                    
                    throw;
                }
                return ranks;
            }
        }
        public List<MilitaryGroup> GetMilitaryGroups(int languageId)
        {
            using (var db = new DataAccess())
            {
                db.CreateStoredProcCommand("dbo.GetMilitaryGroups");
                db.AddInParameter("@LanguageId", DbType.Int32, languageId);

                var groups = new List<MilitaryGroup>();
                try
                {
                    db.ReadInto(groups);
                }
                catch (Exception)
                {
                    throw;
                }
                return groups;
            }
        }
    }
}
