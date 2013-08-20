using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using August2008.Helpers;
using August2008.Models;

namespace August2008
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            DependencyConfig.RegisterDependencyResolver();
            MapperConfig.RegisterMapper();

            ModelBinders.Binders.Add(typeof(DateTime), new DateTimeBinder());
            ModelBinders.Binders.Add(typeof(DateTime?), new NullableDateTimeBinder());

            log4net.Config.XmlConfigurator.Configure(new FileInfo(Server.MapPath("~/log4net.config")));
            log4net.GlobalContext.Properties["Username"] = new UsernameProperty();
        }
        protected void Application_AcquireRequestState(object sender, EventArgs e)  
        {
            var cookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie == null || cookie.Value == "")
            {
                this.SetPrincipal(FormsPrincipal.GetDefaultPrincipal());
                return;
            }
            try
            {
                var ticket = FormsAuthentication.Decrypt(cookie.Value);
                if (ticket != null)
                {
                    var user = ticket.UserData.FromJson<FormsPrincipal>();
                    this.SetPrincipal(user);
                }
            }
            catch
            {
                return;
            }
        }
        private void SetPrincipal(FormsPrincipal principal)
        {
            if (principal != null)
            {
                Context.User = principal;
                var ci = new CultureInfo(principal.Culture);
                Thread.CurrentThread.CurrentUICulture = ci;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
            }
        }
    }
}