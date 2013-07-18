using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace August2008.Common.Interfaces
{
    public interface ICacheProvider
    {
        object this[string key] { get; set; }
        bool Contains(string key);
        bool TryGetObject<T>(string key, out T value);        
        object AddOrGetExisting(string key, object data, CacheItemPolicy policy = null); 
        void Invalidate(string key);
    }
}
