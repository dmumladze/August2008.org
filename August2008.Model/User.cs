using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace August2008.Model
{
    public class User : IPrincipal
    {
        private UserIdentity identity = new UserIdentity();

        public int UserId { get; set; }
        public Language Language { get; set; }

        public IIdentity Identity
        {
            get { return identity; }
        }
        public bool IsInRole(string role)
        {
            return true;
        }
    }
}
