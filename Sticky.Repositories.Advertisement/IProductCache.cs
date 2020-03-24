using Sticky.Models.Mongo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sticky.Repositories.Advertisement
{
    public interface IProductCache
    {
         Task<HostProduct> FindProduct(int hostId, string productId);
        Task UpdateProduct(int hostId, HostProduct hostProduct);
    }
}
