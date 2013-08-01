using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using August2008.Common;
using August2008.Common.Interfaces;
using August2008.Model;

namespace August2008.Data
{
    public class MetadataRepository : IMetadataRepository
    {
        private readonly ICacheProvider Cache;

        public MetadataRepository(ICacheProvider cacheProvder)
        {
            Cache = cacheProvder;
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
    }
}
