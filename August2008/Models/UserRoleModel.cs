using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using August2008.Model;

namespace August2008.Models
{
    public class UserRoleModel
    {
        [Required]
        public int UserId { get; set; }
        public IEnumerable<RoleModel> Roles { get; set; } 
        public IEnumerable<int> PostedRoles { get; set; }
    }
}