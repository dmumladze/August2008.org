using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace August2008.Models
{
    public class HeroModel
    {
        public int? HeroId { get; set; }
        public int? MilitaryRankId { get; set; }
        public int? MilitaryGroupId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime? Dob { get; set; }
        public DateTime? Died { get; set; }
        public string Biography { get; set; }
        public DateTime DateUpdated { get; set; }
        public int UpdatedBy { get; set; }
        public int LanguageId { get; set; }

        public string Thumbnail { get; set; } 
        public IEnumerable<HttpPostedFileBase> Images { get; set; }

        public SelectList MilitaryRanks { get; set; }
        public SelectList MilitaryGroups { get; set; } 
    }
}