using Sticky.Infrastructure.Cache.Models;
using Sticky.Application.Script.Controllers.Dto;


public static class MapperExtension
{
    public static ProductCache ToProductCacheModel(this ProductData product,string hostName)
    {
        return new ProductCache()
        {
            HostName = hostName,
            CategoryName = product.Category,
            Description = product.Description,
            Id = product.ProductId,
            ImageAddress = product.ImageAddress,
            IsAvailable = product.Available,
            Price = product.Price
        };
    }
}

