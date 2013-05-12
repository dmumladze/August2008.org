using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace August2008.Models
{
    public class Hero
    {
        public int HeroId { get; set; }
        public int MilitaryRankId { get; set; }
        public int MilitaryGroupId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Biography { get; set; }
        public DateTime DateUpdated { get; set; }
        public int UpdatedBy { get; set; }
        public int LanguageId { get; set; }
    }
}