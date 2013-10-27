using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using August2008.Common;
using August2008.Common.Interfaces;
using August2008.Model;
using System.IO;
using System.Diagnostics;
using System.Data.SqlClient;
using log4net;

namespace August2008.Data
{
    public sealed class HeroRepository : IHeroRepository
    {
        private readonly ILog Logger;

        public HeroRepository(ILog logger)
        {
            Logger = logger;
        }
        public Hero GetHero(int heroId, int languageId)
        {
            using (var db = new DataAccess())
            {
                db.CreateStoredProcCommand("dbo.GetHero");
                db.AddInputParameter("@HeroId", DbType.Int32, heroId);
                db.AddInputParameter("@LanguageId", DbType.Int32, languageId);
                var hero = new Hero();
                try
                {
                    db.ReadInto(hero, hero.MilitaryGroup, hero.MilitaryRank, hero.MilitaryAward, hero.Photos);
                    GetBlobs(hero, new CloudDataAccess());
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while getting hero", ex);
                    throw;
                }
                return hero;
            }
        }
        public Hero GetRandomHero(int languageId)
        {
            using (var db = new DataAccess())
            {
                db.CreateStoredProcCommand("dbo.GetRandomHero");
                db.AddInputParameter("@LanguageId", DbType.Int32, languageId);
                var hero = new Hero();
                try
                {
                    db.ReadInto(hero, hero.MilitaryGroup, hero.MilitaryRank, hero.MilitaryAward, hero.Photos);
                    GetBlobs(hero, new CloudDataAccess());
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while getting random hero", ex);
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
                    db.AddInputParameter("@FirstName", DbType.String, hero.FirstName);
                    db.AddInputParameter("@LastName", DbType.String, hero.LastName);
                    db.AddInputParameter("@MiddleName", DbType.String, hero.MiddleName);
                    db.AddInputParameter("@Dob", DbType.DateTime, hero.Dob);
                    db.AddInputParameter("@Died", DbType.DateTime, hero.Died);
                    db.AddInputParameter("@MilitaryGroupId", DbType.Int32, hero.MilitaryGroupId);
                    db.AddInputParameter("@MilitaryRankId", DbType.Int32, hero.MilitaryRankId);
                    db.AddInputParameter("@MilitaryAwardId", DbType.Int32, hero.MilitaryAwardId);
                    db.AddInputParameter("@Biography", DbType.String, hero.Biography);
                    db.AddInputParameter("@LanguageId", DbType.Int32, hero.LanguageId);
                    db.AddInputParameter("@UpdatedBy", DbType.Int32, hero.UpdatedBy);
                    db.AddInputParameter("@Photos", DbType.Xml, photos.ToDbXml());
                    db.AddOutputParameter("@HeroId", DbType.Int32);
                    try
                    {
                        db.ExecuteNonQuery();
                        heroId = db.GetParameterValue<int>("@HeroId");
                        SaveBlobs(heroId, photos);
                        tran.Commit();
                    }
                    catch (SqlException ex)
                    {
                        tran.Rollback();
                        Logger.Error("Error while creating hero", ex);
                        throw new RepositoryException("Oops! Something went wrong... :(", ex);
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        Logger.Error("Error while creating hero", ex);
                        throw new RepositoryException("Oops! Something went wrong... :(", ex);
                    }
                    return heroId;
                }
            }
        }
        public void UpdateHero(Hero hero, IEnumerable<IPostedFile> photos)
        {
            using (var tran = new DbTransactionManager())
            {
                try
                {
                    tran.BeginTransaction();
                    using (var db = new DataAccess(tran))
                    {
                        db.CreateStoredProcCommand("dbo.UpdateHero");
                        db.AddInputParameter("@HeroId", DbType.Int32, hero.HeroId);
                        db.AddInputParameter("@FirstName", DbType.String, hero.FirstName);
                        db.AddInputParameter("@LastName", DbType.String, hero.LastName);
                        db.AddInputParameter("@MiddleName", DbType.String, hero.MiddleName);
                        db.AddInputParameter("@Dob", DbType.DateTime, hero.Dob);
                        db.AddInputParameter("@Died", DbType.DateTime, hero.Died);
                        db.AddInputParameter("@MilitaryGroupId", DbType.Int32, hero.MilitaryGroupId);
                        db.AddInputParameter("@MilitaryRankId", DbType.Int32, hero.MilitaryRankId);
                        db.AddInputParameter("@MilitaryAwardId", DbType.Int32, hero.MilitaryAwardId);
                        db.AddInputParameter("@Biography", DbType.String, hero.Biography);
                        db.AddInputParameter("@LanguageId", DbType.Int32, hero.LanguageId);
                        db.AddInputParameter("@UpdatedBy", DbType.Int32, hero.UpdatedBy);
                        db.AddInputParameter("@Photos", DbType.Xml, photos.ToDbXml());

                        db.ExecuteNonQuery();
                        SaveBlobs(hero.HeroId.Value, photos);
                        tran.Commit();
                    }
                }
                catch (Exception ex)
                {                    
                    tran.Rollback();
                    Logger.Error("Error while updating hero", ex);
                    throw;
                }
            }
        }
        public HeroPhoto DeletePhoto(int heroPhotoId)
        {
            using (var db = new DataAccess())
            {
                db.CreateStoredProcCommand("dbo.DeleteHeroPhoto");
                db.AddInputParameter("@HeroPhotoId", DbType.Int32, heroPhotoId);
                var photo = new HeroPhoto();
                try
                {
                    db.ReadInto(photo);
                    DeleteBlob(photo);
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while deleting hero", ex);
                    throw;
                }
                return photo;
            }
        }
        public HeroSearchCriteria SearchHeros(HeroSearchCriteria criteria)
        {
            var heros = new List<Hero>();
            var photos = new List<HeroPhoto>();

            using (var db = new DataAccess())
            {
                db.CreateStoredProcCommand("dbo.GetHeros");
                db.AddInputParameter("@PageNo", DbType.Int32, criteria.PageNo);
                db.AddInputParameter("@Name", DbType.String, criteria.Name);
                db.AddInputParameter("@PageSize", DbType.Int32, criteria.PageSize);
                db.AddInputParameter("@LanguageId", DbType.Int32, criteria.LanguageId);
                db.AddOutputParameter("@TotalCount", DbType.Int32);
                try
                {
                    db.ReadInto(heros, photos);
                    heros.ForEach(x => x.Photos = photos.Where(y => y.HeroId == x.HeroId));
                    GetBlobs(heros, new CloudDataAccess());
                    criteria.Result = heros;
                    criteria.TotalCount = db.GetParameterValue<int>("@TotalCount");
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while searching hero", ex);
                    throw;
                }
                return criteria;
            }
        }
        public IEnumerable<string> GetAlphabet(int languageId)
        {
            using (var db = new DataAccess())
            {
                db.CreateStoredProcCommand("dbo.GetHeroAlphabet");
                db.AddInputParameter("@LanguageId", DbType.String, languageId);
                try
                {
                    var alphabet = new List<string>();
                    db.ReadInto(alphabet);
                    return alphabet;
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while getting alphabet", ex);
                    throw;
                }
            }
        }
        private void SaveBlobs(int heroId, IEnumerable<IPostedFile> photos)
        {
            var cloud = new CloudDataAccess();
            foreach (var item in photos)
            {
                cloud.AddBlob("images", string.Format("hero/{0}/{1}", heroId, Path.GetFileName(item.FileName)), item.Stream, item.ContentType);
            }
        }
        private void GetBlobs(IEnumerable<Hero> heros, CloudDataAccess cloud)
        {
            foreach (var item in heros)
            {
                GetBlobs(item, cloud);
            }
        }
        private void GetBlobs(Hero hero, CloudDataAccess cloud)
        {
            if (hero.HeroId.HasValue)
            {
                var baseUri = cloud.GetBaseUri("images/hero", hero.HeroId.ToString());
                GetBlobs(baseUri, hero.Photos);
            }
        }
        private void GetBlobs(Uri baseUri, IEnumerable<HeroPhoto> photos)
        {
            foreach (var item in photos)
            {
                if (!string.IsNullOrWhiteSpace(item.PhotoUri))
                {
                    item.PhotoUri = Path.Combine(baseUri.AbsoluteUri, item.PhotoUri);
                }
            }
        }
        private void DeleteBlob(HeroPhoto photo)
        {
            var cloud = new CloudDataAccess();
            cloud.DeleteBlob("images/hero", photo.HeroId.ToString(), photo.PhotoUri);
        }
    }
}
