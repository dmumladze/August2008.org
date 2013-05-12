using System;
using System.Collections.Generic;

namespace August2008.Model 
{ 
    public class Hero
    {
        public int HeroId { get; set; }
        public int MilitaryRankId { get; set; }
        public int MilitaryGroupId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime? Dob { get; set; }
        public DateTime? Died { get; set; }
        public string Biography { get; set; }
        public DateTime DateUpdated { get; set; }
        public int UpdatedBy { get; set; }
        public int LanguageId { get; set; }

        public MilitaryRank MilitaryRank { get; set; }
        public MilitaryGroup MilitaryGroup { get; set; }

        public HeroPhoto Thumbnail { get; set; } 
        public List<HeroPhoto> Photos { get; set; }
    }
}