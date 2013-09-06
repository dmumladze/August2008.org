using System;
using System.IO;
using System.Web;
using August2008.Models;
using log4net;
using log4net.Config;

namespace August2008
{
    public class LoggerConfig
    {
        public static void RegisterLog4Net()
        {
            XmlConfigurator.Configure();
            GlobalContext.Properties["Username"] = new UsernameProperty();
        }
    }
}