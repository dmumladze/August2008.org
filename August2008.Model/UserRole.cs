using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace August2008.Model
{
    public class UserRole
    {
        public int UserId { get; set; }
        public List<int> Roles { get; set; } 
    }
}
