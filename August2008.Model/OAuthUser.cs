using System;
using System.Collections.Generic;

namespace August2008.Model
{
    public class OAuthUser
    {
        public int OAuthUserId { get; set; }
        public string ProviderId { get; set; }
        public string ProviderName { get; set; }
        public IDictionary<string, string> ProviderData { get; set; }
    }
}
