using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sticky.Repositories.Dashboard
{
    public interface IMemCache
    {
        bool StoreValue<T>(string key, T value);

        T GetValue<T>(string key);
    }
}
