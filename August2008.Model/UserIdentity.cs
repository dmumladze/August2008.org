using System;
using System.Security.Principal;

namespace August2008.Model
{
    public class UserIdentity : IIdentity
    {        
        public string AuthenticationType
        {
            get { return "Forms Authentication"; }
        }
        public bool IsAuthenticated
        {
            get { return true; }
        }
        public string Name
        {
            get { return "David Mumladze"; }
        }
    }
}
