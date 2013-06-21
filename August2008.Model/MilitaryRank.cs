using System;

namespace August2008.Model 
{ 
    public class MilitaryRank
    {
        public int MilitaryRankId { get; set; }
        public string RankName { get; set; }
        public string Description { get; set; }
        public int LanguageId { get; set; }
    }
}