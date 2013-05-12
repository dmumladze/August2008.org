using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using August2008.Model;

namespace August2008.Models
{
    public class HeroModel
    {
        public Hero Hero { get; set; }
        public SelectList MilitaryRanks { get; set; }
        public SelectList MilitaryGroups { get; set; } 
    }
}