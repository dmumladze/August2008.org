namespace August2008.Common.Interfaces
{
    public interface IDbConnectionResolver
    {
        string Name { get; }
        string ProviderName { get; }
        string ConnectionString { get; }
    }
}
