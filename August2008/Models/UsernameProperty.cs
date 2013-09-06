using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace August2008.Models
{
    public class UsernameProperty
    {
        public override string ToString()
        {
            if (!HttpContext.Current.IsNull())
            {
                return HttpContext.Current.User.Identity.Name;
            }
            return string.Empty;
        }
    }
}