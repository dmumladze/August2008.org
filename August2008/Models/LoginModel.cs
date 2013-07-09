using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace August2008.Models
{
    public class LoginModel
    {
        public string Provider { get; set; }
        public bool LoginSuccessful { get; set; }
        public string ReturnUrl { get; set; }

    }
}