using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Routing;

namespace August2008.Helpers
{
    public class CultureHelper
    {
        public static void SetCurrentCulture(string culture)
        {
            var info = CultureInfo.CreateSpecificCulture(culture);
            Thread.CurrentThread.CurrentCulture = info;
            Thread.CurrentThread.CurrentUICulture = info;  
        }
    }
}