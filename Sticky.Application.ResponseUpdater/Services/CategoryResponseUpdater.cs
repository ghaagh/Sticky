using Microsoft.Extensions.Options;
using Sticky.Domain.ResponseAggrigate;
using Sticky.Infrastructure.Cache;
using Sticky.Infrastructure.Cache.Models;
using Sticky.Infrastructure.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sticky.Application.ResponseUpdater.Services
{
    public class CategoryResponseUpdater : IResponseUpdater
    {
        private readonly IMultipleCache<SegmentCache> _segmentCache;
        private readonly IResponseRepositoy _responseRepositoy;
        private readonly IRequestRepository _requestRepository;
        private readonly Setting _setting;
        public CategoryResponseUpdater(IResponseRepositoy responseRepositoy, IMultipleCache<SegmentCache> segmentCache, IOptions<Setting> options, IRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
            _setting = options.Value;
            _segmentCache = segmentCache;
            _responseRepositoy = responseRepositoy;
        }

        public async Task DoAsync()
        {
            while (true)
            {
                Dictionary<long, List<MessageModel>> usersDictionary = new Dictionary<long, List<MessageModel>>();
                List<long> userIdBatchList = new List<long>();
                var segment = await _segmentCache.GetListAsync();
                var categorySegment = segment.Where(c => !string.IsNullOrEmpty(c.ActivityExtra)).ToList();
                var categories = categorySegment.Select(c => c.ActivityExtra);
                var count = 1;
                while (_setting.BatchSize>count)
                {
                    var item = await _requestRepository.GetLast(ResponseUpdaterTypeEnum.Category);
                    if (string.IsNullOrEmpty(item))
                    {
                        break;
                    }
                    var parseResult = long.TryParse(item, out long userId);
                    if (!parseResult)
                        continue;
                    userIdBatchList.Add(userId);
                    count++;
                }
                if (userIdBatchList.Count == 0)
                {
                    Thread.Sleep(4000);
                    continue;
                }
                var restClient = new RestSharp.RestClient(_setting.DruidClient)
                {
                    Timeout = 4000
                };
                var request3 = new RestSharp.RestRequest("druid/v2/sql", RestSharp.Method.POST);


                request3.AddJsonBody(new
                {
                    query = CreateDruidRequestBody(categories, userIdBatchList)
                }) ;
                var responseList = restClient.Execute<List<MessageModel>>(request3);
                if (responseList.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var grouped = responseList.Data.GroupBy(c => long.Parse(c.UserId));
                    foreach (var item in grouped)
                    {
                        if (item.Count() != 0)
                            usersDictionary.TryAdd(item.Key, item.ToList());
                    }
                }
                foreach (var item in userIdBatchList)
                {
                    var found = false;
                    List<MessageModel> userList = new List<MessageModel>();
                    found = usersDictionary.TryGetValue(item, out userList);
                    if (found)
                    {
                        var cats = userList.Select(c => c.CategoryName);
                        var segmentIds = categorySegment.Where(c => cats.Contains(c.ActivityExtra));
                        List<Membership> finalSegmentsForRedis = new List<Membership>();
                        if (segmentIds != null)
                        {

                            finalSegmentsForRedis.AddRange(segmentIds.Select(c => new Membership()
                            {
                                HostId = _setting.HostId ?? 0,
                                SegmentId = c.Id
                            }));
                        }
                        await _responseRepositoy.SetMembership(ResponseUpdaterTypeEnum.Category, item,
                            finalSegmentsForRedis, _setting.EmptyExpirationInMinutes, _setting.FullExpirationInMinutes);

                    }
                }
                Console.Clear();

            }

        }

        private string CreateDruidRequestBody(IEnumerable<string> categories, List<long> userBatchList)
        {
            return $"select Distinct(CategoryName),UserId from ProductActions WHERE UserId in " +
                                  $"({string.Join(',', userBatchList.Select(c => "'" + c + "'"))}) " +
                                  $"AND CategoryName In({string.Join(',',categories)}) GROUP BY CategoryName,UserId limit 200";


        }
    }
}
