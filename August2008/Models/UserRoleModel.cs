using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using August2008.Model;

namespace August2008.Models
{
    public class UserRoleModel
    {
        [Required]
        public int UserId { get; set; }
        public IEnumerable<Role> PostedRoles { get; set; }
        public IEnumerable<Role> UserRoles { get; set; }
        public MultiSelectList AvaialbleRoles { get; set; }
    }
}