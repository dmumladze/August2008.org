using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using August2008.Model;

namespace August2008.Models
{
    public class UserSearchModel
    {
        public IEnumerable<UserModel> Users { get; set; }
        public IEnumerable<Role> AvailableRoles { get; set; }
    }
}