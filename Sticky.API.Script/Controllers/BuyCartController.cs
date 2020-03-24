using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Sticky.Dto.Script.Request;
using Sticky.Models.Druid;
using Sticky.Models.Etc;
using Sticky.Repositories.Advertisement;
using Sticky.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sticky.API.Script.Controllers
{
    [Produces("application/json")]
    [Route("BuyCart")]
    public class BuyCartController : Controller
    {


        private readonly ScriptAPISetting _configurations;
        private readonly IHostCache _hostDictionary;
        private readonly IKafkaLogger _kafkaLogger;
        private readonly IErrorLogger _errorLogger;
        private readonly IHostScriptChecker _hostScriptChecker;
        private readonly IProductCache _productCache;
        public BuyCartController(IHostScriptChecker hostScriptChecker, IProductCache productCache, IKafkaLogger kafkaLogger, IErrorLogger errorLogger, IOptions<ScriptAPISetting> options, IHostCache hostDictionary)
        {
            _kafkaLogger = kafkaLogger;
            _configurations = options.Value;
            _hostScriptChecker = hostScriptChecker;
            _productCache = productCache;
            _errorLogger = errorLogger;
            _hostDictionary = hostDictionary;
        }

        [HttpPost]
        public async Task UpdateProducts([FromBody]RemoveProductLog logData)
        {
            try
            {
                var productIds = logData.ProductData.Select(c => c.ProductId);
                var productAddress = new Uri(logData.PageAddress).PathAndQuery;
                var host = await _hostDictionary.GetHostByIdAsync(logData.HostId);
                if (host == null || host.Id == 0)
                    return;
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
                            return;
                    }
                }
                #endregion
                await _hostScriptChecker.UpdateBuyValidation(logData.HostId);

                foreach (var item in productIds)
                {
                    var cachedProduct = await _productCache.FindProduct(logData.HostId, item);
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
                            ProductId = item,
                            StatType = StatTypes.ProductPurchase,
                            UserId = logData.UserId.ToString(),
                            HostId = logData.HostId.ToString(),

                        });
                    }

                }
            }
            catch (Exception ex)
            {
                await _errorLogger.LogError("Operation:Buy =>" + ex.Message);
            }
        }
    }
}