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
        IEnumerable<MilitaryRank> GetMilitaryRanks(int languageId);
        IEnumerable<MilitaryGroup> GetMilitaryGroups(int languageId);
        IEnumerable<Role> GetRoles();
    }
}
