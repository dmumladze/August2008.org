using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace August2008.Helpers
{
    public class CultureRoute : Route
    {
        public CultureRoute(string url, object defaults, object contraints)
            : base(url, new MvcRouteHandler())
        {
            base.Defaults = CreateRouteValueDictionary(defaults);
            base.Constraints = CreateRouteValueDictionary(contraints);
        }
        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            return base.GetVirtualPath(requestContext, values);
        }
        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var routeData = base.GetRouteData(httpContext);
            if (routeData != null)
            {
                var culture = routeData.Values["culture"].ToString();
                var cookie = httpContext.Request.Cookies["culture"];
                var areEqual = false;
                if (cookie == null || cookie.Value == "" || !(areEqual = string.Equals(culture, cookie.Value, StringComparison.OrdinalIgnoreCase)))
                {
                    routeData.Values["culture"] = culture;
                    httpContext.Response.Cookies.Add(new HttpCookie("culture", culture));                    
                }
                else if (!areEqual)
                {                    
                    routeData.Values["culture"] = cookie.Value;
                }
                CultureHelper.SetCurrentCulture(culture);
            }            
            return routeData;
        }
        private static RouteValueDictionary CreateRouteValueDictionary(object values)
        {
            var dictionary = values as IDictionary<string, object>;
            if (dictionary != null)
            {
                return new RouteValueDictionary(dictionary);
            }
            else
            {
                return new RouteValueDictionary(values);
            }
        }  
    }
}