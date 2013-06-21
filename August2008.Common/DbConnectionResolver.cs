using System;
using System.Configuration;
using August2008.Common.Interfaces;

namespace August2008.Common
{
    public sealed class DbConnectionResolver : IDbConnectionResolver
    {
        public readonly static IDbConnectionResolver Default = new DbConnectionResolver();

        private DbConnectionResolver()
        {
            var settings = ConfigurationManager.ConnectionStrings["August2008Db"];
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
