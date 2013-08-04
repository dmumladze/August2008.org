using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using Newtonsoft.Json;

namespace August2008.Models
{
    public class FormsPrincipal : IPrincipal
    {
        public FormsPrincipal()
        {            
        }
        public FormsPrincipal(IIdentity identity)
        {
            Identity = identity;
        }

        public int UserId { get; set; }
        public string Email { get; set; }
        public int LanguageId { get; set; }
        public string Culture { get; set; }
        public bool SuperUser { get; set; }
        public List<string> Roles { get; set; }
        [JsonProperty(TypeNameHandling = TypeNameHandling.Objects)]
        public IIdentity Identity { get; private set; }

        public bool IsInRole(string role)
        {
            return (SuperUser || (!Roles.IsNullOrEmpty() && Roles.Contains(role, StringComparer.OrdinalIgnoreCase)));
        }

        internal static FormsPrincipal GetDefaultPrincipal()
        {
            return new FormsPrincipal
                {
                    LanguageId = 1,
                    Culture = "ka-GE",
                    Identity = new FormsIdentity2("Anonymous")
                };
        }
    }
}