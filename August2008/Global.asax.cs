using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using August2008.Model;
using August2008.Models;
using AutoMapper;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.Unity.Mvc;

namespace August2008
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            var container = new UnityContainer();
            var section = ConfigurationManager.GetSection("unity.mvc") as UnityConfigurationSection;
            if (section != null)
            {
                section.Configure(container);
            }
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            Mapper.CreateMap<HeroModel, Hero>();
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            var ci = new CultureInfo("ka");
            System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);

            var app = sender as HttpApplication;
            if (app != null)
            {
                app.Context.User = new User
                    {
                        UserId = 2,
                        Profile = new UserProfile { Lang = new Language { LanguageId = 1 } }
                    };
            }
        }
    }
}