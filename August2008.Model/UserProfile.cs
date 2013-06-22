using System;
using System.Collections.Generic;

namespace August2008.Model
{
    public class UserProfile
    {
        public UserProfile()
        {
            Lang = new Language();
        }
        public DateTime? Dob { get; set; }
        public string Nationality { get; set; }
        public Language Lang { get; set; }
    }
}
