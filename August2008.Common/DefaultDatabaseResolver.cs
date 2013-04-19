using System;
using System.Configuration;
using MelaniArt.Core.Interfaces;

namespace MelaniArt.Core.DataAccess
{
    public sealed class DefaultDatabaseResolver : IDbConnectionResolver
    {
        public readonly static IDbConnectionResolver Instance = new DefaultDatabaseResolver();

        private DefaultDatabaseResolver()
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["DefaultDatabase"];
            if (settings == null)
                throw new ArgumentException("Default database connection has not been setup");
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
