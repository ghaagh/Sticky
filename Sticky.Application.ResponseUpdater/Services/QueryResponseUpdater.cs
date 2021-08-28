using Microsoft.Extensions.Options;
using Sticky.Domain.ResponseAggrigate;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aerospike.Client;
using Sticky.Infrastructure.Cache.Models;
using System.Linq;
using Sticky.Infrastructure.Cache;

namespace Sticky.Application.ResponseUpdater.Services
{
    public class QueryResponseUpdater : IResponseUpdater
    {
        private readonly IAsyncClient _aeroClient;
        private readonly ICache<ProductCache> _productCache;
        private readonly ICache<HostCache> _hostCache;
        private readonly IResponseRepositoy _responseRepositoy;
        private readonly IRequestRepository _requestRepository;
        private readonly Setting _setting;
        public QueryResponseUpdater(IResponseRepositoy responseRepositoy, ICache<ProductCache> productCache, ICache<HostCache> hostCache, IAsyncClient aeroCLient, IOptions<Setting> options, IRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
            _setting = options.Value;
            _productCache = productCache;
            _hostCache = hostCache;
            _aeroClient = aeroCLient;
            _responseRepositoy = responseRepositoy;
        }

        public async Task DoAsync()
        {
            while (true)
            {
                var usersDictionary = new Dictionary<string, List<Membership>>();
                var batchRequestId = await PrepareBatchRequestAsync();
                if (batchRequestId.Count == 0)
                {
                    Thread.Sleep(4000);
                    continue;
                }
                foreach (var item in batchRequestId)
                {
                    var membershipList = new List<Membership>();
                    var matchKey = new Key("Sticky", "Activity", $"{ item }_{_setting.HostId}");
                    var record = _aeroClient.Get(new Policy(), matchKey);
                    if (record != null)
                    {

                        var segmentMembership = new Membership()
                        {
                            HostId = _setting.HostId ?? 0,
                            SegmentId = _setting.SegmentId ?? 0
                           
                        };
                        var r = record.GetValue(_setting.StatType);
                        var productIds = new List<string>();
                        if (r != null)
                        {
                            productIds = r.ToString().Split(",").Where(c => c != "").ToList();
                        }
                        if (productIds.Count != 0)
                        {
                            foreach (var productId in productIds.OrderBy(c => Guid.NewGuid()).Take(5))
                            {
                                var host = await _hostCache.GetAsync(_setting.HostId.Value.ToString());
                                var productData = await _productCache.GetAsync($"{host.Host}_{productId}");
                                if (productData != null && !string.IsNullOrEmpty(productData.ProductName) && !string.IsNullOrEmpty(productData.Url) && !string.IsNullOrEmpty(productData.ImageAddress) && productData.IsAvailable && productData.Price != 0)
                                    segmentMembership.Products.Add(new MemberShipProduct { ProductId = productData.Id, Image = productData.ImageAddress, ProductName = productData.ProductName, UrlAddress = productData.Url, Price = productData.Price });
                            }
                            membershipList.Add(segmentMembership);
                        }

                    }
                    await _responseRepositoy.SetMembership(ResponseUpdaterTypeEnum.SpecialSegment, long.Parse(item), membershipList, _setting.EmptyExpirationInMinutes, _setting.FullExpirationInMinutes, _setting.SegmentId.ToString());

                }


            }

        }

        private async Task<List<string>> PrepareBatchRequestAsync()
        {
            var batchRequestIds = new List<string>();
            while (batchRequestIds.Count <= _setting.BatchSize)
            {
                var batchItem = await _requestRepository.GetLast(ResponseUpdaterTypeEnum.SpecialSegment, _setting.SegmentId.ToString());
                var parseResult = long.TryParse(batchItem, out long userId);
                if (!parseResult)
                    continue;
                if (string.IsNullOrEmpty(batchItem))
                {
                    Thread.Sleep(4000);
                    break;
                }

                if (await _responseRepositoy.ExistAsync(ResponseUpdaterTypeEnum.SpecialSegment, userId, _setting.SegmentId.ToString()))
                    continue;

                batchRequestIds.Add(batchItem);
            }
            return batchRequestIds;
        }
    }
}
