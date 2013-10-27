using System;
using System.Configuration;
using August2008.Common;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace August2008.Tests.Helpers
{
    public class UnityHelper
    {
        public static IUnityContainer container;

        public static void Setup()
        {
            container = new UnityContainer();
            var section = ConfigurationManager.GetSection("unity.mvc") as UnityConfigurationSection;
            if (section != null)
            {
                section.Configure(container);
            }
            container.AddNewExtension<BuildTrackingExtension>().AddNewExtension<LogCreationExtension>();
        }
        public static T Resolve<T>()
        {
            return container.Resolve<T>();
        }
        public static void Destroy()
        {
            container.Dispose();
        }
    }
}
