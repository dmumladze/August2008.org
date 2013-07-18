using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace August2008.Model
{
    public class User 
    {
        public User()
        {
            Profile = new UserProfile();
            OAuth = new OAuthUser();
            Roles = new List<Role>();
        }        
        public int UserId { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public DateTime MemeberSince { get; set; }
        public bool SuperAdmin { get; set; }
        public string Password { get; set; }

        public UserProfile Profile { get; set; }
        public OAuthUser OAuth { get; set; }

        public List<Role> Roles { get; set; } 
    }
}
