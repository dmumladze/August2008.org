using System;
using System.Collections.Generic;
using August2008.Model;

namespace August2008.Common.Interfaces
{
    public interface IHeroRepository
    {
        Hero GetHero(int heroId, int languageId);
        Hero GetRandomHero(int languageId);
        int CreateHero(Hero hero, IEnumerable<IPostedFile> photos);
        void UpdateHero(Hero hero, IEnumerable<IPostedFile> photos);  
        HeroSearchCriteria SearchHeros(HeroSearchCriteria criteria);
        HeroPhoto DeletePhoto(int heroPhotoId);
        IEnumerable<string> GetAlphabet(int languageId);
    }
}
