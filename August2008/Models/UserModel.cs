using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using August2008.Model;

namespace August2008.Models
{
    public class UserModel
    {
        public IEnumerable<User> Users { get; set; }
        public IEnumerable<Role> Roles { get; set; }

        [Required]
        public int UserId { get; set; }
        public List<int> AssignedRoles { get; set; }
        public List<int> RevokedRoles { get; set; }
    }
}