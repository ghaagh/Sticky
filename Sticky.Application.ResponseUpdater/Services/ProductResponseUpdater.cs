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
using Sticky.Infrastructure.Message;
using Sticky.Domain.SegmentAggrigate;

namespace Sticky.Application.ResponseUpdater.Services
{
    public class ProductResponseUpdater : IResponseUpdater
    {
        private readonly IAsyncClient _aeroClient;
        private readonly ICache<ProductCache> _productCache;
        private readonly IMultipleCache<SegmentCache> _segmentCache;
        private readonly IResponseRepositoy _responseRepositoy;
        private readonly IRequestRepository _requestRepository;
        private readonly Setting _setting;
        public ProductResponseUpdater(IResponseRepositoy responseRepositoy, IMultipleCache<SegmentCache> segmentCache, ICache<ProductCache> productCache, IAsyncClient aeroCLient, IOptions<Setting> options, IRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
            _setting = options.Value;
            _segmentCache = segmentCache;
            _productCache = productCache;
            _aeroClient = aeroCLient;
            _responseRepositoy = responseRepositoy;
        }

        public async Task DoAsync()
        {

            while (true)
            {
                var activeSegments = await _segmentCache.GetListAsync();
                var pagedSegments = activeSegments.Where(c => c.ActivityType == ActivityTypeEnum.VisitPage).Select(c => c.Id.ToString());
                var productSegment = activeSegments.Where(c => c.ActivityType != ActivityTypeEnum.VisitPage && c.ActivityExtra == null).ToList();
                var activeHostIds = productSegment.Select(c => c.HostId).Distinct();

                var usersDictionary = new Dictionary<long, List<MessageModel>>();
                var item = await _requestRepository.GetLast(ResponseUpdaterTypeEnum.ProductAndPage);
                if (string.IsNullOrEmpty(item))
                {
                    Thread.Sleep(4000);
                    continue;
                }
                var parseResult = long.TryParse(item, out long userId);
                if (await _responseRepositoy.ExistAsync(ResponseUpdaterTypeEnum.ProductAndPage,userId))
                    continue;

                var finalSegmentsForRedis = new List<Membership>();
                var userData = new Dictionary<long, Dictionary<string, object>>();
                foreach (var activeHost in activeHostIds)
                {
                    var matchKey = new Key("Sticky", "Activity", $"{ item }_{activeHost}");
                    var record = _aeroClient.Get(new Policy(), matchKey);
                    if (record == null)
                        continue;
                    Dictionary<string, object> dic = record.bins;
                    userData.TryAdd(activeHost, dic);
                }
                if (userData.Count == 0)
                {
                    await _responseRepositoy.SetMembership(ResponseUpdaterTypeEnum.ProductAndPage, userId, finalSegmentsForRedis,
                        _setting.EmptyExpirationInMinutes, _setting.FullExpirationInMinutes);
                    continue;

                }
                foreach (var segment in productSegment)
                {
                    var foundinDataDic = false;
                    var userDataforhost = new Dictionary<string, object>();

                    foundinDataDic = userData.TryGetValue(segment.HostId, out userDataforhost);
                    if (!foundinDataDic)
                        continue;
                    var foundSegmentDatainUserdata = false;
                    var segmentData = new object();
                    foundSegmentDatainUserdata = userDataforhost.TryGetValue(segment.ActivityType.ToString(), out segmentData);
                    if (!foundSegmentDatainUserdata)
                        continue;
                    var productIds = new List<string>();
                    var productVisitSegments = new Membership()
                    {
                        HostId = segment.HostId,
                        SegmentId = segment.Id
                    };

                    if (segment.ActionType == ActionTypeEnum.Same)
                    {
                        productIds = segmentData.ToString().Split(",").Where(c => c != "").Distinct().ToList();
                        if (productIds.Count != 0)
                        {
                            foreach (var productId in productIds.OrderBy(c => Guid.NewGuid()).Take(5))
                            {
                                var productData = await _productCache.GetAsync($"{segment.HostId}_{productId}");
                                if (productData != null && !string.IsNullOrEmpty(productData.ProductName)
                                    && !string.IsNullOrEmpty(productData.Url) &&
                                    !string.IsNullOrEmpty(productData.ImageAddress) &&
                                    productData.IsAvailable && productData.Price != 0)
                                    productVisitSegments.Products.Add(new MemberShipProduct
                                    {
                                        ProductId = productData.Id,
                                        Image = productData.ImageAddress,
                                        ProductName = productData.ProductName,
                                        UrlAddress = productData.Url,
                                        Price = productData.Price
                                    });
                            }

                        }
                    }
                    finalSegmentsForRedis.Add(productVisitSegments);
                }
                foreach (var hosts in activeHostIds)
                {
                    var foundinDataDic = false;
                    var userDataforhost = new Dictionary<string, object>();

                    foundinDataDic = userData.TryGetValue(hosts, out userDataforhost);
                    if (!foundinDataDic)
                        continue;
                    var foundSegmentDatainUserdata = false;
                    var segmentData = new object();
                    foundSegmentDatainUserdata = userDataforhost.TryGetValue("PagedMemberships", out segmentData);
                    if (!foundSegmentDatainUserdata)
                        continue;
                    if (segmentData == null)
                        continue;
                    var segmentMembershipList = segmentData.ToString().Split(",");
                    if (segmentMembershipList != null && !segmentMembershipList.Any())
                        continue;
                    var productVisitSegments = new List<Membership>();
                    foreach (var item1 in segmentMembershipList)
                    {
                        if (pagedSegments.Contains(item1))
                            productVisitSegments.Add(new Membership()
                            {
                                HostId = hosts,
                                SegmentId = int.Parse(item1)
                            });
                    }
                    var membership = new List<string>();
                    finalSegmentsForRedis.AddRange(productVisitSegments);


                }
                await _responseRepositoy.SetMembership(ResponseUpdaterTypeEnum.ProductAndPage,userId, finalSegmentsForRedis,
                    _setting.EmptyExpirationInMinutes,
                    _setting.FullExpirationInMinutes);


            }
        }
    }
}
