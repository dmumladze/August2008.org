using System;
using System.Collections.Generic;
using August2008.Model;

namespace August2008.Common.Interfaces
{
    public interface IHeroRepository
    {
        Hero GetHero(int heroId, int languageId);
        int CreateHero(Hero hero);

        List<MilitaryRank> GetMilitaryRanks(int languageId);
        List<MilitaryGroup> GetMilitaryGroups(int languageId);
    }
}
