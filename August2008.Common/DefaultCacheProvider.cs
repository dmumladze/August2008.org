using System;
using System.Runtime.Caching;
using August2008.Common.Interfaces;

namespace August2008.Common
{
    public class DefaultCacheProvider : ICacheProvider
    {
        private static ObjectCache Cache { get { return MemoryCache.Default; } }

        public bool Contains(string key)
        {
            return Cache.Contains(key);
        }
        public bool TryGetObject<T>(string key, out T value)
        {
            value = default(T);
            if (Cache.Contains(key))
            {
                var obj = Cache[key];
                if (obj != null)
                {
                    value = (T)Cache[key];
                    return true;
                }
            }
            return false;
        }

        public object this[string key]
        {
            get { return Cache[key]; }
            set { Cache[key] = value; }
        }
        public object AddOrGetExisting(string key, object data, CacheItemPolicy policy = null) 
        {
           return Cache.AddOrGetExisting(key, data, policy);
        }
        public void Invalidate(string key)
        {
            Cache.Remove(key);
        }
    }
}
