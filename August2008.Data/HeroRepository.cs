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
        public int CreateHero(Hero hero, IEnumerable<IPostedFile> photos)
        {
            var heroId = 0;
            using (var ts = new DbTransactionManager())
            {
                ts.BeginTransaction();

                using (var db = new DataAccess(ts))
                {
                    db.CreateStoredProcCommand("dbo.CreateHero");

                    db.AddInParameter("@FirstName", DbType.String, hero.FirstName);
                    db.AddInParameter("@LastName", DbType.String, hero.LastName);
                    db.AddInParameter("@MiddleName", DbType.String, hero.MiddleName);
                    db.AddInParameter("@Dob", DbType.DateTime, hero.Dob);
                    db.AddInParameter("@Died", DbType.DateTime, hero.Died);
                    db.AddInParameter("@MilitaryGroupId", DbType.Int32, hero.MilitaryGroupId);
                    db.AddInParameter("@MilitaryRankId", DbType.Int32, hero.MilitaryRankId);
                    db.AddInParameter("@LanguageId", DbType.Int32, hero.LanguageId);
                    db.AddInParameter("@UpdatedBy", DbType.Int32, hero.UpdatedBy);
                    db.AddInParameter("@Photos", DbType.Xml, photos.ToDbXml());
                    db.AddOutParameter("@HeroId", DbType.Int32);
                    try
                    {
                        db.ExecuteNonQuery();
                        heroId = db.GetParameterValue<int>("@HeroId");
                        ts.Commit();
                        SavePhotos(photos);
                    }
                    catch (Exception)
                    {
                        ts.Rollback();
                    }
                    return heroId;
                }
            }
        }
        public IEnumerable<MilitaryRank> GetMilitaryRanks(int languageId)
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
        public IEnumerable<MilitaryGroup> GetMilitaryGroups(int languageId)
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
        private static void SavePhotos(IEnumerable<IPostedFile> photos)
        {
            if (photos == null) return;
            try
            {
                foreach (var photo in photos)
                {
                    photo.Save();
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
