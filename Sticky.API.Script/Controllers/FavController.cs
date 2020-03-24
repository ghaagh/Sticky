using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Sticky.Dto.Script.Request;
using Sticky.Models.Druid;
using Sticky.Models.Etc;
using Sticky.Repositories.Advertisement;
using Sticky.Repositories.Common;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sticky.API.Script.Controllers
{
    [Produces("application/json")]
    
    public class FavController : Controller
    {

        private readonly ScriptAPISetting _configurations;

        private readonly IKafkaLogger _kafkaLogger;
        private readonly IErrorLogger _errorLogger;
        private readonly IHostCache _hostCache;
        private readonly IHostScriptChecker _hostScriptChecker;
        private readonly IProductCache _productCache;
        public FavController(IProductCache productCache,IHostScriptChecker hostScriptChecker,IKafkaLogger kafkaLogger,IHostCache hostCache, IErrorLogger errorLogger,IOptions<ScriptAPISetting> options)
        {
            _kafkaLogger = kafkaLogger;
            _hostScriptChecker = hostScriptChecker;
            _configurations = options.Value;
            _hostCache = hostCache;
            _productCache = productCache;
            _errorLogger = errorLogger;
        }
        [Route("Unlike")]
        [HttpPost]
        public async Task<string> Unlike([FromBody]RemoveProductLog logData)
        {

            try
            {
                var host = await _hostCache.GetHostByIdAsync(logData.HostId);
                if (host == null || host.Id == 0)
                    return CommonStrings.NoHost;
                #region SecurityCheck
                if (_configurations.SecurityCheck)
                {
                    var origin = Request.Headers[CommonStrings.Origin].ToString();
                    if (!string.IsNullOrEmpty(origin))
                    {
                        var aut = new Uri(origin).Host;
                        aut = aut.ToLower();
                        var topdomain = string.Empty;
                        if (aut.IndexOf(CommonStrings.Dot) == aut.LastIndexOf(CommonStrings.Dot))
                            topdomain = aut;
                        else
                            topdomain = aut.Substring(aut.IndexOf(CommonStrings.Dot) + 1);
                        if (host.Host != topdomain)
                            return CommonStrings.NoHostAccess;
                    }
                }
                #endregion
                await _hostScriptChecker.UpdateFavValidation(logData.HostId);

                var productad = logData.ProductData.OrderByDescending(c => c.ProductId).ToList();
                if (!productad.Any())
                    return string.Empty;
                foreach (var item in productad)
                {
                    var cachedProduct = await _productCache.FindProduct(logData.HostId, item.ProductId);
                    if (cachedProduct.Id != string.Empty)
                    {
                        await _kafkaLogger.SendMessage(new DruidData()
                        {
                            CategoryName = cachedProduct.CategoryName,
                            Date = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:sszzz"),
                            HostName = host.Host,
                            ImageAddress = cachedProduct.ImageAddress,
                            Price = cachedProduct.Price,
                            ProductName = cachedProduct.ProductName,
                            PageAddress = cachedProduct.Url,
                            ProductId = item.ProductId,
                            StatType = StatTypes.Like,
                            UserId = logData.UserId.ToString(),
                            HostId = logData.HostId.ToString(),

                        });
                    }
                }

            }
            catch (Exception ex)
            {
                await _errorLogger.LogError("Operations:AdToCart->" + ex.Message);
               
            } return string.Empty;
        }
        [Route("Like")]
        [HttpPost]
        public  async Task<string> Like([FromBody]RemoveProductLog logData)
        {
            try
            {
                var host = await _hostCache.GetHostByIdAsync(logData.HostId);
                if (host == null || host.Id == 0)
                    return CommonStrings.NoHost;
                #region SecurityCheck
                if (_configurations.SecurityCheck)
                {
                    var origin = Request.Headers[CommonStrings.Origin].ToString();
                    if (!string.IsNullOrEmpty(origin))
                    {
                        var aut = new Uri(origin).Host;
                        aut = aut.ToLower();
                        var topdomain = string.Empty;
                        if (aut.IndexOf(CommonStrings.Dot) == aut.LastIndexOf(CommonStrings.Dot))
                            topdomain = aut;
                        else
                            topdomain = aut.Substring(aut.IndexOf(CommonStrings.Dot) + 1);
                        if (host.Host != topdomain)
                            return CommonStrings.NoHostAccess;
                    }
                }
                #endregion
                await _hostScriptChecker.UpdateFavValidation(logData.HostId);


                var productad = logData.ProductData.OrderByDescending(c => c.ProductId).ToList();
                if (!productad.Any())
                    return string.Empty;
                foreach (var item in productad)
                {
                    var cachedProduct = await _productCache.FindProduct(logData.HostId, item.ProductId);
                    if (cachedProduct.Id != string.Empty)
                    {
                        await _kafkaLogger.SendMessage(new DruidData()
                        {
                            CategoryName = cachedProduct.CategoryName,
                            Date = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:sszzz"),
                            HostName = host.Host,
                            ImageAddress = cachedProduct.ImageAddress,
                            Price = cachedProduct.Price,
                            ProductName = cachedProduct.ProductName,
                            PageAddress = cachedProduct.Url,
                            ProductId = item.ProductId,
                            StatType = StatTypes.Like,
                            UserId = logData.UserId.ToString(),
                            HostId = logData.HostId.ToString(),

                        });
                    }
                }

            }
            catch (Exception ex)
            {
                await _errorLogger.LogError("Operations:Fav->" + ex.Message);
            }
            return string.Empty;
        }
    }
}