using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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
    [Route("AdToCart")]
    public class AdToCartController : Controller
    {
        private readonly IHostCache _hostCache;
        private readonly IKafkaLogger _kafkaLogger;
        private readonly ITotalVisitUpdater _totalVisitUpdater;
        private readonly ScriptAPISetting _configurations;
        private readonly IErrorLogger _errorLogger;
        private readonly IProductCache _productCache;
        private readonly IHostScriptChecker _hostScriptChecker;
        public AdToCartController(IOptions<ScriptAPISetting> conf,IHostScriptChecker hostScriptChecker,IKafkaLogger kafkaLogger,IProductCache productCache,IErrorLogger errorLogger,ITotalVisitUpdater totalVisitUpdater,IHostCache hostCache)
        {
            _kafkaLogger = kafkaLogger;
            _errorLogger = errorLogger;
            _hostScriptChecker = hostScriptChecker;
            _hostCache = hostCache;
            _productCache = productCache;
            _totalVisitUpdater = totalVisitUpdater;
            _configurations = conf.Value;

        }

        [HttpPost]
        public async Task<string> UpdateProducts([FromBody]AddToCartLog logData)
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
                        var topdomain = string.Empty ;
                        if (aut.IndexOf(CommonStrings.Dot) == aut.LastIndexOf(CommonStrings.Dot))
                            topdomain = aut;
                        else
                            topdomain = aut.Substring(aut.IndexOf(CommonStrings.Dot) + 1);
                        if (host.Host != topdomain)
                            return CommonStrings.NoHostAccess;
                    }
                }
                #endregion

                var productad = logData.ProductData.OrderByDescending(c => c.ProductId).ToList();
                if (!productad.Any())
                    return string.Empty;
                await _hostScriptChecker.UpdateCartValidation(logData.HostId);
                await _totalVisitUpdater.UpdateTotalVisit(logData.HostId);
              
                   foreach(var item in productad)
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
                            StatType = StatTypes.AddToCart,
                            UserId = logData.UserId.ToString(),
                            HostId = logData.HostId.ToString(),

                        });
                    }
                }


            }
            catch (Exception ex)
            {
                await _errorLogger.LogError("Operations:AdToCart->" + ex.Message);
            }



            return "";






        }
    }
}