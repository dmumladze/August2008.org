using System;
using System.Collections.Generic;
using August2008.Model;

namespace August2008.Common.Interfaces
{
    public interface IHeroRepository
    {
        Hero GetHero(int heroId, int languageId);
        int CreateHero(Hero hero, IEnumerable<IPostedFile> photos);

        IEnumerable<MilitaryRank> GetMilitaryRanks(int languageId);
        IEnumerable<MilitaryGroup> GetMilitaryGroups(int languageId);
    }
}
