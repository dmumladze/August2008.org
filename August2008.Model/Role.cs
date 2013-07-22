using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace August2008.Model
{
    public class Role
    {
        public int RoleId { get; set; }
        public int? UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } 
    }
}
