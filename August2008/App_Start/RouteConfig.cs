using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using August2008.Helpers;

namespace August2008
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "Partial",
            //    url: "{culture}/{cotroller}/partial/{view}",
            //    defaults: new { culture = "ka", controller = "home", action = "partial", view = "" },
            //    constraints: new { culture = "(ka|en)" }
            //);
            //routes.MapRoute(
            //    name: "Default",
            //    url: "{culture}/{controller}/{action}/{id}",
            //    defaults: new { culture = "ka", controller = "home", action = "index", id = UrlParameter.Optional},
            //    constraints: new { culture = "(ka|en)" }
            //);

            routes.Add("Partial", new CultureRoute(
                "{culture}/{cotroller}/partial/{view}",
                new { culture = "ka", controller = "home", action = "partial", view = "" },
                new { culture = "(ka|en)" }));

            routes.Add("Default", new CultureRoute(
                "{culture}/{controller}/{action}/{id}",
                new { culture = "ka", controller = "home", action = "index", id = UrlParameter.Optional },
                new { culture = "(ka|en)" }));
        }
    }
}