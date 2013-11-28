using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using August2008.Model;

namespace August2008.Common.Interfaces
{
    public interface IMetadataRepository
    {
        IEnumerable<Language> GetLanguages();
        IEnumerable<MilitaryRank> GetMilitaryRanks(int languageId);
        IEnumerable<MilitaryGroup> GetMilitaryGroups(int languageId);
        IEnumerable<MilitaryAward> GetMilitaryAwards(int languageId); 

        IEnumerable<Role> GetRoles();

        bool TryGetCountry(string country, out Country match);
        bool TryGetState(string state, string country, out State match);
        bool TryGetCity(string city, string state, string postalCode, string country, out City match); 

        Country CreateCountry(Country country);
        State CreateState(State state);
        City CreateCity(City city);  
    }
}
