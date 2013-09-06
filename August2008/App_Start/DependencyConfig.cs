using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using August2008.Common;
using August2008.Controllers;
using August2008.Helpers;
using August2008.Services;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.Unity.Mvc;

namespace August2008
{
    public class DependencyConfig
    {
        public static void RegisterDependencyResolver()
        {
            var container = new UnityContainer();
            var section = ConfigurationManager.GetSection("unity.mvc") as UnityConfigurationSection;
            if (section != null)
            {
                section.Configure(container);
            }

            container
                .AddNewExtension<BuildTrackingExtension>()
                .AddNewExtension<LogCreationExtension>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}