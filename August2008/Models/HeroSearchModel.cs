using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace August2008.Models
{
    public class HeroSearchModel
    {
        public IEnumerable<HeroModel> Result { get; set; }
        public IEnumerable<AlphabetLetter> Alphabet { get; set; }

        public int PageNo { get; set; }
        public string Name { get; set; }
        public int PageSize { get; set; }
        public int LanguageId { get; set; }
        public int TotalCount { get; set; }
    }
}