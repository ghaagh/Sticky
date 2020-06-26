using Sticky.Dto.Script.Request;
using Sticky.Models.Druid;
using Sticky.Models.Etc;
using Sticky.Models.Mongo;
using Sticky.Repositories.Advertisement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sticky.Repositories.Script.Implementions
{
    public class KafkaLogProducer : IKafkaLogProducer
    {
        private readonly IProductCache _productCache;
        private readonly IKafkaClient _kafkaClient;
        public KafkaLogProducer(IProductCache productCache,IKafkaClient kafkaClient)
        {
            _kafkaClient = kafkaClient;
            _productCache = productCache;

        }

        public async Task GeneratePageLog(PageLogRequest pageLogRequest, string host)
        {
            var innerAddress = new Uri(pageLogRequest.Address).PathAndQuery;
            await _kafkaClient.SendMessage(new DruidData()
            {
                CategoryName = "",
                Date = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:sszzz"),
                HostName = host,
                ImageAddress = "",
                Price = 0,
                ProductName = "",
                PageAddress = innerAddress,
                ProductId = "",
                StatType = StatTypes.PageView,
                UserId = pageLogRequest.UserId.ToString(),
                HostId = pageLogRequest.HostId.ToString(),

            });
        }

        public async Task GenerateProductLogFromId(string statType, ModifyProductLog logData,string host)
        {
            var productad = logData.ProductData.OrderByDescending(c => c.ProductId).ToList();
            foreach (var item in productad)
            {
                var cachedProduct = await _productCache.FindProduct(logData.HostId, item.ProductId);
                if (cachedProduct.Id != string.Empty)
                {
                    await _kafkaClient.SendMessage(new DruidData()
                    {
                        CategoryName = cachedProduct.CategoryName,
                        Date = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:sszzz"),
                        HostName = host,
                        ImageAddress = cachedProduct.ImageAddress,
                        Price = cachedProduct.Price,
                        ProductName = cachedProduct.ProductName,
                        PageAddress = cachedProduct.Url,
                        ProductId = item.ProductId,
                        StatType = statType,
                        UserId = logData.UserId.ToString(),
                        HostId = logData.HostId.ToString(),

                    });
                }
            }
        }

        public async Task GenerateProductLogFromProducts(string statType, ProductLog productlog ,string host)
        {
            var fiProducts = productlog.ProductData.OrderByDescending(c => c.ProductId).Take(20);
            List<Task> tasks = new List<Task>();
            foreach (var item in fiProducts)
            {
                var updatedProduct = new HostProduct()
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
                tasks.Add(_productCache.UpdateProduct(productlog.HostId, updatedProduct));
            }
            foreach (var item in productlog.ProductData)
            {
                await _kafkaClient.SendMessage(new DruidData()
                {
                    CategoryName = item.Category,
                    Date = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:sszzz"),
                    HostName = host,
                    ImageAddress = item.ImageAddress,
                    Price = item.Price,
                    ProductName = item.Name,
                    PageAddress = item.PageAddress,
                    ProductId = item.ProductId,
                    StatType = StatTypes.ProductView,
                    UserId = productlog.UserId.ToString(),
                    HostId = productlog.HostId.ToString(),

                });
            }
            await Task.WhenAll(tasks).ConfigureAwait(false);
        }
    }
}
