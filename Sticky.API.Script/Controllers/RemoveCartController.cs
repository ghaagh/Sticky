
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Sticky.Repositories.Advertisement;
using Sticky.Models.Etc;
using Sticky.Dto.Script.Request;
using Sticky.Models.Druid;

namespace Sticky.API.Script.Controllers
{
    [Produces("application/json")]
    [Route("RemoveCart")]
    public class RemoveCartController : Controller
    {
        private readonly IHostCache _hostCache;
        private readonly IProductCache _productCache;
        private readonly IKafkaLogger _kafkaLogger;
        private readonly ScriptAPISetting _configurations;
        public RemoveCartController(IProductCache productCache, IKafkaLogger kafkaLogger,IHostCache hostCache, IOptions<ScriptAPISetting> options)
        {
            _kafkaLogger = kafkaLogger;
            _hostCache = hostCache;
            _productCache = productCache;
            _configurations = options.Value;
        }

        [HttpPost]
        public  async Task UpdateProducts([FromBody]RemoveProductLog logData)
        {
            try
            {
            var productIds = logData.ProductData.Select(c => c.ProductId);
            var productAddress = new Uri(logData.PageAddress).PathAndQuery;
                var host =await  _hostCache.GetHostByIdAsync(logData.HostId);
            
            if (host==null || host.Id == 0)
                return;
                #region SecurityCheck
                if (_configurations.SecurityCheck)
                {
                    var origin = Request.Headers["Origin"].ToString();
                    if (!string.IsNullOrEmpty(origin))
                    {
                        var aut = new Uri(origin).Host;
                        aut = aut.ToLower();
                        var topdomain = ""; ;
                        if (aut.IndexOf(".") == aut.LastIndexOf("."))
                            topdomain = aut;
                        else
                            topdomain = aut.Substring(aut.IndexOf(".") + 1);
                        if (host.Host != topdomain)
                            return;
                    }
                }
                #endregion

               foreach(var item in productIds)
                {
     
                    var cachedProduct = await _productCache.FindProduct(logData.HostId, item);
                    if (cachedProduct.Id != string.Empty)
                    {
                       await  _kafkaLogger.SendMessage(new DruidData()
                        {
                            CategoryName = cachedProduct.CategoryName,
                            Date = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:sszzz"),
                            HostName = host.Host,
                            ImageAddress = cachedProduct.ImageAddress,
                            Price = cachedProduct.Price,
                            ProductName = cachedProduct.ProductName,
                            PageAddress = cachedProduct.Url,
                            ProductId = item,
                            StatType = StatTypes.RemoveFromCart,
                            UserId = logData.UserId.ToString(),
                            HostId = logData.HostId.ToString(),

                        });
                    }
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}