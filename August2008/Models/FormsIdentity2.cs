using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace August2008.Models
{
    public class FormsIdentity2 : IIdentity
    {
        public FormsIdentity2(string name, bool isAuthenticated = false)
        {
            Name = name;
            IsAuthenticated = isAuthenticated;
        }
        public string AuthenticationType
        {
            get { return "Forms Authentication"; }
        }
        public bool IsAuthenticated { get; private set; }
        public string Name { get; private set; }
    }
}