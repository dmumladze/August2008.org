using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace August2008.Helpers
{
    public static class UrlHelper2
    {
        public static string ImageUrl(this UrlHelper helper, string fileName)
        {
            return helper.ResolveUrl("~/images/" + fileName);
        }
        public static string ResolveUrl(this UrlHelper helper, string url)
        {
            return VirtualPathUtility.ToAbsolute(url);
        }
    }
}