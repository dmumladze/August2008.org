using System;

namespace August2008.Common
{
    public interface IDbConnectionResolver
    {
        string Name { get; }
        string ProviderName { get; }
        string ConnectionString { get; }
    }
}
