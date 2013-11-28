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
        public static string GetCurrentCulture(bool ignoreRouteData = false)
        {
            if (!ignoreRouteData)
            {
                var routeData = HttpContext.Current.Request.RequestContext.RouteData;
                object culture;
                if (routeData.Values.TryGetValue("culture", out culture))
                {
                    return culture.ToString();
                }
            }
            var cookie = HttpContext.Current.Request.Cookies["culture"];
            if (cookie != null && cookie.Value != null)
            {
                return cookie.Value;
            }
            return GetThreadCulture();
        }
        public static string GetThreadCulture()
        {
            var culture = Thread.CurrentThread.CurrentCulture.Name;
            if (culture.IndexOf('-') > -1)
            {
                culture = culture.Substring(0, 2);
            }
            return culture;
        }
    }
}