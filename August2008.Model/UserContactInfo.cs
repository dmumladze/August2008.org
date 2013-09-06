using System;

namespace August2008.Model
{
    public class UserContactInfo
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }

        public Address Address { get; set; }
    }
}
