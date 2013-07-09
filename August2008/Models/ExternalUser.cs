using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace August2008.Models
{
    public class ExternalUser
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Link { get; set; }
        public string Gender { get; set; }
        public string AccessToken { get; set; }
        public string Provider { get; set; }
        public string UniqueUserId { get; set; }
    }
}