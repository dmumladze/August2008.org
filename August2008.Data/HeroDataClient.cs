using System;
using System.Collections.Generic;
using System.Data;
using August2008.Common;
using August2008.Model;

namespace August2008.Data
{
    public sealed class HeroDataClient
    {
        public List<MilitaryRank> GetMilitaryRanks(int languageId)
        {
            using (var database = new Database())
            {
                database.CreateStoredProcCommand("dbo.GetMilitaryRanks");
                database.AddInParameter("@LanguageId", DbType.Int32, languageId);

                var ranks = new List<MilitaryRank>();
                try
                {
                    database.ReadInto(ranks);
                }
                catch (Exception)
                {                    
                    throw;
                }
                return ranks;
            }
        }
    }
}
