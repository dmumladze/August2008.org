using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace August2008.Model
{
    public class User : IPrincipal
    {
        private UserIdentity _identity;

        public User()
        {
            Profile = new UserProfile();
            OAuth = new OAuthUser();
            Roles = new List<string>();
        }        
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public DateTime MemeberSince { get; set; }

        public UserProfile Profile { get; set; }
        public OAuthUser OAuth { get; set; }

        public List<string> Roles { get; set; } 

        public IIdentity Identity
        {
            get
            {
                 return _identity ?? (_identity = new UserIdentity
                            {
                                AuthenticationType = OAuth.ProviderName,
                                IsAuthenticated = !string.IsNullOrWhiteSpace(OAuth.ProviderId),
                                Name = DisplayName ?? UserName ?? Email
                            });
            }
        }
        public bool IsInRole(string role)
        {
            return Roles.Contains(role, StringComparer.OrdinalIgnoreCase);
        }
    }
}
