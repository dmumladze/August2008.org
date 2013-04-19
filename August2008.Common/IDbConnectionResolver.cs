using System;

namespace MelaniArt.Core.Interfaces
{
    public interface IDbConnectionResolver
    {
        string Name { get; }
        string ProviderName { get; }
        string ConnectionString { get; }
    }
}
