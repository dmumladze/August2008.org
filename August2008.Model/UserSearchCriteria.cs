using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace August2008.Model
{
    public class UserSearchCriteria
    {
        public IEnumerable<User> Users { get; set; }        
        
        public string StartsWith { get; set; }
        public bool IncludeRoles { get; set; }
    }
}
