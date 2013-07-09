using System;
using System.Linq;
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
                db.CreateStoredProcCommand("dbo.GetHero");
                db.AddInParameter("@HeroId", DbType.Int32, heroId);
                db.AddInParameter("@LanguageId", DbType.Int32, languageId);
                var hero = new Hero();
                try
                {
                    db.ReadInto(hero, hero.MilitaryGroup, hero.MilitaryRank, hero.Photos);
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
            using (var tran = new DbTransactionManager())
            {
                tran.BeginTransaction();
                using (var db = new DataAccess(tran))
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
                        tran.Commit();
                    }
                    catch (Exception)
                    {
                        tran.Rollback();
                    }
                    return heroId;
                }
            }
        }
        public HeroPhoto DeletePhoto(int heroPhotoId)
        {
            using (var db = new DataAccess())
            {
                db.CreateStoredProcCommand("dbo.DeleteHeroPhoto");
                db.AddInParameter("@HeroPhotoId", DbType.Int32, heroPhotoId);

                var photo = new HeroPhoto();
                try
                {
                    db.ReadInto(photo);
                }
                catch (Exception)
                {
                    throw;
                }
                return photo;
            }
        }
        public HeroSearchCriteria GetHeros(HeroSearchCriteria criteria)
        {
            var heros = new List<Hero>();
            var photos = new List<HeroPhoto>();

            using (var db = new DataAccess())
            {
                db.CreateStoredProcCommand("dbo.GetHeros");

                db.AddInParameter("@PageNo", DbType.Int32, criteria.PageNo);
                db.AddInParameter("@PageSize", DbType.Int32, criteria.PageSize);
                db.AddInParameter("@LanguageId", DbType.Int32, criteria.LanguageId);
                db.AddOutParameter("@TotalCount", DbType.Int32);

                try
                {
                    db.ReadInto(heros, photos);
                    heros.ForEach(x => x.Photos = photos.Where(y => y.HeroId == x.HeroId));
                    criteria.Result = heros;
                }
                catch (Exception)
                {
                    throw;
                }
                return criteria;
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
    }
}
