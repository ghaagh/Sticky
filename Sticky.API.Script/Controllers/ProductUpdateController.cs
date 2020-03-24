using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Sticky.Dto.Script.Request;
using Sticky.Models.Druid;
using Sticky.Models.Etc;
using Sticky.Models.Mongo;
using Sticky.Repositories.Advertisement;
using Sticky.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sticky.API.Script.Controllers
{
    [Produces("application/json")]
    [Route("ProductUpdate")]
    public class ProductUpdateController : Controller
    {
        private readonly IHostCache _hostCache;
        private readonly ITotalVisitUpdater _totalVisitUpdater;
        private readonly ICategoryLogger _categoryLogger;
        private readonly IKafkaLogger _kafkaLogger;
        private readonly IHostScriptChecker _hostScriptChecker;
        private readonly IProductCache _productCache;
        private readonly ICrowlerCache _crowlerCache;
        private readonly IErrorLogger _errorLogger;
        private readonly ScriptAPISetting _configurations;
        public ProductUpdateController(IHostCache hostCache, IHostScriptChecker hostScriptChecker, IProductCache productCache,  ICrowlerCache crowlerCache, IKafkaLogger kafkaLogger, ICategoryLogger categoryLogger, IErrorLogger errorLogger, IOptions<ScriptAPISetting> options, ITotalVisitUpdater totalVisitUpdater)
        {
            _kafkaLogger = kafkaLogger;
            _categoryLogger = categoryLogger;
            _hostScriptChecker = hostScriptChecker;
            _productCache = productCache;
            _errorLogger = errorLogger;
            _crowlerCache = crowlerCache;
            _configurations = options.Value;
            _hostCache = hostCache;
            _totalVisitUpdater = totalVisitUpdater;
        }

        [HttpPost]
        public async Task<string> UpdateProducts([FromBody]ProductLog logData)
        {
            try
            {

                var fiProducts = logData.ProductData.OrderByDescending(c => c.ProductId).Take(20);
                var host = await _hostCache.GetHostByIdAsync(logData.HostId).ConfigureAwait(false);
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
                if (logData != null && (await _crowlerCache.IsCrowler(logData.UserId)))
                    return string.Empty;
                List<Task> tasks = new List<Task>
                {
                    _totalVisitUpdater.UpdateTotalVisit(logData.HostId)
                };
                await _hostScriptChecker.UpdateProductValidation(logData.HostId);
                var categoriesforlog = logData.ProductData.Where(c => !string.IsNullOrEmpty(c.Category)).GroupBy(c => c.Category).Select(v => new KeyValuePair<string, int>(v.Key, v.Count()));
                foreach (var category in categoriesforlog)
                {
                    tasks.Add(_categoryLogger.LogCategory(host.Id, category.Key, category.Value));
                }
                foreach (var item in fiProducts)
                {
                    var updateedProduct = new HostProduct()
                    {
                        Description = item.Description,
                        ImageAddress = item.ImageAddress,
                        IsAvailable = item.Available,
                        Price = item.Price,
                        Url = item.PageAddress,
                        Id = item.ProductId,
                        ProductName = item.Name,
                        CategoryName = item.Category,
                        UpdateDate = DateTime.Now
                    };
                    tasks.Add(_productCache.UpdateProduct(logData.HostId, updateedProduct));
                }

                #region Log Into Kafka
                foreach (var item in logData.ProductData)
                {
                    await _kafkaLogger.SendMessage(new DruidData()
                    {
                        CategoryName = item.Category,
                        Date = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:sszzz"),
                        HostName = host.Host,
                        ImageAddress = item.ImageAddress,
                        Price = item.Price,
                        ProductName = item.Name,
                        PageAddress = item.PageAddress,
                        ProductId = item.ProductId,
                        StatType = StatTypes.ProductView,
                        UserId = logData.UserId.ToString(),
                        HostId = logData.HostId.ToString(),

                    });
                }
                #endregion

                await Task.WhenAll(tasks).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await _errorLogger.LogError("Operation:ProductUpdate =>" + ex.Message);
            }
            return string.Empty;
        }







    }
}

