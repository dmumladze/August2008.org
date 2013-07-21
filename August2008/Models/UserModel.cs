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
        public int UserId { get; set; }
        public string DisplayName { get; set; }
        public DateTime MemeberSince { get; set; }
        public IEnumerable<Role> Roles { get; set; }
    }
}