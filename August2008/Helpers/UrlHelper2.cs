using System;
using System.Configuration;
using System.Net.Http;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;

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
        public static string PayPalAction(this UrlHelper helper)
        {
#if DEBUG
            return "https://www.sandbox.paypal.com/cgi-bin/webscr";
#else
            return ConfigurationManager.AppSettings["PayPal:WebScrUrl"]; 
#endif                    
        }
        public static string ExternalAction(this UrlHelper helper, string actionName, string controllerName = null, RouteValueDictionary routeValues = null, string protocol = null)
        {
#if DEBUG
            var client = new HttpClient();
            var ipAddress = client.GetStringAsync("http://ipecho.net/plain").Result; 
            var route = UrlHelper.GenerateUrl(null, actionName, controllerName, routeValues, helper.RouteCollection, helper.RequestContext, true); 
            if (route == null)
            {
                return route;
            }
            if (string.IsNullOrEmpty(protocol) && string.IsNullOrEmpty(ipAddress))
            {
                return route;
            }
            var url = HttpContext.Current.Request.Url;
            protocol = !string.IsNullOrWhiteSpace(protocol) ? protocol : Uri.UriSchemeHttp;
            return string.Concat(protocol, Uri.SchemeDelimiter, ipAddress, route);
#else
            return helper.Action(actionName, null, null, HttpContext.Current.Request.Url.Scheme);
#endif
        }
    }
}