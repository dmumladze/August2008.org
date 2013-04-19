using System;
using System.Configuration;

namespace August2008.Common
{
    public sealed class DbConnectionResolver : IDbConnectionResolver
    {
        public readonly static IDbConnectionResolver Instance = new DbConnectionResolver();

        private DbConnectionResolver()
        {
            var settings = ConfigurationManager.ConnectionStrings["DefaultDatabase"];
            if (settings == null)
            {
                throw new ArgumentException("Default database connection has not been setup");
            }
            this.Name = settings.Name;
            this.ProviderName = settings.ProviderName;
            this.ConnectionString = settings.ConnectionString;
        }
        public string Name
        {
            get;
            private set;
        }
        public string ProviderName
        {
            get;
            private set;
        }
        public string ConnectionString
        {
            get;
            private set;
        }
    }
}
