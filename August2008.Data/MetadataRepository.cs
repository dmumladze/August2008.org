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
                    db.AddInputParameter("@LanguageId", DbType.Int32, languageId);
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
                    db.AddInputParameter("@LanguageId", DbType.Int32, languageId);
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
                    db.AddInputParameter("@LanguageId", DbType.Int32, languageId);
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
                db.AddInputParameter("@Country", DbType.String, country);
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
                db.AddInputParameter("@State", DbType.String, state);
                db.AddInputParameter("@Country", DbType.String, country);
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
        public bool TryGetCity(string city, string state, string postalCode, string country, out City match)
        {
            using (var db = new DataAccess())
            {
                db.CreateStoredProcCommand("dbo.TryGetCity");
                db.AddInputParameter("@City", DbType.String, city);
                db.AddInputParameter("@State", DbType.String, state);
                db.AddInputParameter("@PostalCode", DbType.String, postalCode);
                db.AddInputParameter("@Country", DbType.String, country);
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
                db.AddInputParameter("@Name", DbType.String, country.Name);
                db.AddInputParameter("@FullName", DbType.String, country.FullName);
                db.AddInputParameter("@Latitude", DbType.Double, country.Latitude);
                db.AddInputParameter("@Longitude", DbType.Double, country.Longitude);
                db.AddOutputParameter("@CountryId", DbType.Int32);
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
                db.AddInputParameter("@CountryId", DbType.Int32, state.CountryId);
                db.AddInputParameter("@Country", DbType.String, state.Country);
                db.AddInputParameter("@Name", DbType.String, state.Name);
                db.AddInputParameter("@FullName", DbType.String, state.FullName);
                db.AddInputParameter("@Latitude", DbType.Double, state.Latitude);
                db.AddInputParameter("@Longitude", DbType.Double, state.Longitude);
                db.AddOutputParameter("@StateId", DbType.Int32);
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
                db.AddInputParameter("@StateId", DbType.Int32, city.StateId);
                db.AddInputParameter("@State", DbType.String, city.State);
                db.AddInputParameter("@Name", DbType.String, city.Name);
                db.AddInputParameter("@PostalCode", DbType.String, city.PostalCode);                
                db.AddInputParameter("@Latitude", DbType.Double, city.Latitude);
                db.AddInputParameter("@Longitude", DbType.Double, city.Longitude);
                db.AddOutputParameter("@CityId", DbType.Int32);
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
