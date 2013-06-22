using System;
using System.Security.Principal;

namespace August2008.Model
{
    public class UserIdentity : IIdentity
    {
        public string AuthenticationType { get; set; }
        public bool IsAuthenticated { get; set; }
        public string Name { get; set; }
    }
}
