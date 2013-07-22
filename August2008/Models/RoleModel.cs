using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace August2008.Models
{
    public class RoleModel
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
        public int? UserId { get; set; }
        public bool Assigned { get { return UserId.HasValue; } }
    }
}