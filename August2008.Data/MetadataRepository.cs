using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using August2008.Common;
using August2008.Common.Interfaces;
using August2008.Model;
using log4net;

namespace August2008.Data
{
    public class MetadataRepository : IMetadataRepository
    {
        private readonly ICacheProvider Cache;
        private readonly ILog Logger;

        public MetadataRepository(ICacheProvider cache, ILog logger)
        {
            Cache = cache;
            Logger = logger;
        }
        public IEnumerable<MilitaryRank> GetMilitaryRanks(int languageId)
        {
            List<MilitaryRank> ranks;
            if (!Cache.TryGetObject("MilitaryRanks" + languageId, out ranks))
            {
                using (var db = new DataAccess())
                {
                    db.CreateStoredProcCommand("dbo.GetMilitaryRanks");
                    db.AddInParameter("@LanguageId", DbType.Int32, languageId);
                    ranks = new List<MilitaryRank>();
                    try
                    {
                        db.ReadInto(ranks);
                    }
                    catch (Exception)
                    {
                        throw;
                    }                    
                }
            }
            return ranks;
        }
        public IEnumerable<MilitaryGroup> GetMilitaryGroups(int languageId)
        {
            List<MilitaryGroup> groups;
            if (!Cache.TryGetObject("MilitaryGroups" + languageId, out groups))
            {
                using (var db = new DataAccess())
                {
                    db.CreateStoredProcCommand("dbo.GetMilitaryGroups");
                    db.AddInParameter("@LanguageId", DbType.Int32, languageId);
                    groups = new List<MilitaryGroup>();
                    try
                    {
                        db.ReadInto(groups);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            return groups;
        }
        public IEnumerable<MilitaryAward> GetMilitaryAwards(int languageId)
        {
            List<MilitaryAward> awards;
            if (!Cache.TryGetObject("MilitaryAwards" + languageId, out awards))
            {
                using (var db = new DataAccess())
                {
                    db.CreateStoredProcCommand("dbo.GetMilitaryAwards");
                    db.AddInParameter("@LanguageId", DbType.Int32, languageId);
                    awards = new List<MilitaryAward>();
                    try
                    {
                        db.ReadInto(awards);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            return awards;
        }
        public IEnumerable<Role> GetRoles()
        {
            List<Role> roles;
            if (!Cache.TryGetObject("Roles", out roles))
            {
                using (var db = new DataAccess())
                {
                    db.CreateStoredProcCommand("dbo.GetRoles");
                    roles = new List<Role>();
                    try
                    {
                        db.ReadInto(roles);
                    }
                    catch (Exception)
                    {
                        throw;
                    }                    
                }
            }
            return roles;
        }
        public bool TryGetCountry(string country, out Country match)
        {
            using (var db = new DataAccess())
            {
                db.CreateStoredProcCommand("dbo.TryGetCountry");
                db.AddInParameter("@Country", DbType.String, country);
                db.AddReturnParameter();
                try
                {
                    db.ReadInto(match = new Country());
                    return Convert.ToBoolean(db.GetReturnValue());
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while getting country.", ex);
                }
            }
            match = default(Country);
            return false;
        }
        public bool TryGetState(string state, string country, out State match)
        {
            using (var db = new DataAccess())
            {
                db.CreateStoredProcCommand("dbo.TryGetState");
                db.AddInParameter("@State", DbType.String, state);
                db.AddInParameter("@Country", DbType.String, country);
                db.AddReturnParameter();
                try
                {
                    db.ReadInto(match = new State());
                    return Convert.ToBoolean(db.GetReturnValue());
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while getting state.", ex);
                }
            }
            match = default(State);
            return false;
        }
        public bool TryGetCity(string city, string state, string country, out City match)
        {
            using (var db = new DataAccess())
            {
                db.CreateStoredProcCommand("dbo.TryGetCity");
                db.AddInParameter("@City", DbType.String, city);
                db.AddInParameter("@State", DbType.String, state);
                db.AddInParameter("@Country", DbType.String, country);
                db.AddReturnParameter();
                try
                {
                    db.ReadInto(match = new City());
                    return Convert.ToBoolean(db.GetReturnValue());
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while getting country.", ex);
                }
            }
            match = default(City);
            return false;
        }
        public Country CreateCountry(Country country)
        {
            using (var db = new DataAccess())
            {
                db.CreateStoredProcCommand("dbo.CreateCountry");
                db.AddInParameter("@Name", DbType.String, country.Name);
                db.AddInParameter("@FullName", DbType.String, country.FullName);
                db.AddInParameter("@Latitude", DbType.Double, country.Latitude);
                db.AddInParameter("@Longitude", DbType.Double, country.Longitude);
                db.AddOutParameter("@CountryId", DbType.Int32);
                try
                {
                    db.ExecuteNonQuery();
                    country.CountryId = db.GetParameterValue<int>("@CountryId");
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while creating country.", ex);
                    throw;
                }
            }
            return country;
        }
        public State CreateState(State state)
        {
            using (var db = new DataAccess())
            {
                db.CreateStoredProcCommand("dbo.CreateState");
                db.AddInParameter("@CountryId", DbType.Int32, state.CountryId);
                db.AddInParameter("@Country", DbType.String, state.Country);
                db.AddInParameter("@Name", DbType.String, state.Name);
                db.AddInParameter("@FullName", DbType.String, state.FullName);
                db.AddInParameter("@Latitude", DbType.Double, state.Latitude);
                db.AddInParameter("@Longitude", DbType.Double, state.Longitude);
                db.AddOutParameter("@StateId", DbType.Int32);
                try
                {
                    db.ExecuteNonQuery();
                    state.StateId = db.GetParameterValue<int>("@StateId");
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while creating state.", ex);
                    throw;
                }
            }
            return state;
        }
        public City CreateCity(City city)
        {
            using (var db = new DataAccess())
            {
                db.CreateStoredProcCommand("dbo.CreateCity");
                db.AddInParameter("@StateId", DbType.Int32, city.StateId);
                db.AddInParameter("@State", DbType.String, city.State);
                db.AddInParameter("@Name", DbType.String, city.Name);
                db.AddInParameter("@PostalCode", DbType.String, city.PostalCode);                
                db.AddInParameter("@Latitude", DbType.Double, city.Latitude);
                db.AddInParameter("@Longitude", DbType.Double, city.Longitude);
                db.AddOutParameter("@CityId", DbType.Int32);
                try
                {
                    db.ExecuteNonQuery();
                    city.CityId = db.GetParameterValue<int>("@CityId");
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while creating city.", ex);
                    throw;
                }
            }
            return city;
        }
    }
}
