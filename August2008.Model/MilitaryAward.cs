using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace August2008.Model
{
    public class MilitaryAward
    {
        public int MilitaryAwardId { get; set; }
        public string AwardName { get; set; }
        public string Description { get; set; }
        public int LanguageId { get; set; } 
    }
}
