using Microsoft.AspNetCore.Mvc;
using Sticky.Infrastructure.Cache;
using Sticky.Infrastructure.Cache.Models;
using Sticky.Infrastructure.Message;
using Sticky.Application.Script.Controllers.Dto;
using Sticky.Application.Script.Services;
using System.Threading.Tasks;

namespace Sticky.Application.Script.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUtility _domainExtractor;
        private readonly IMessage _messager;
        private readonly ICacheUpdater<ProductCache> _cacheUpdater;

        public ProductController(IUtility domainExtractor, IMessage messager, ICacheUpdater<ProductCache> cacheUpdater)
        {
            _domainExtractor = domainExtractor;
            _messager = messager;
            _cacheUpdater = cacheUpdater;
        }

        [HttpPost]
        public async Task<ActionResult> Post(ProductRequest request)
        {
            var origin = Request.Headers["Origin"].ToString();
            var hostAddress = _domainExtractor.ExtractDomain(origin);
            foreach (var product in request.Products)
            {
                await _messager.SendProductMessageAsync(hostAddress, request.UserId, product.ProductId, product.Name, product.ImageAddress, product.Category, product.PageAddress, product.Price);
                var productCache = product.ToProductCacheModel(hostAddress);
                await _cacheUpdater.UpdateOneAsync($"{hostAddress}_{productCache.Id}",productCache);
            }
            return Ok();
        }
    }
}
