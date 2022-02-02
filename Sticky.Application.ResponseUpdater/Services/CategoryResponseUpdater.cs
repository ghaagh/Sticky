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

namespace Sticky.Application.ResponseUpdater.Services;

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
            var userIdBatchList = await ExtractUserIdsFromRequestCacheAsync();
            if (userIdBatchList.Count == 0)
            {
                Thread.Sleep(4000);
                continue;
            }

            var categorySegment = await GetCategorySegmentsAsync();

            var response = await new DruidQueryService(_setting.DruidClient)
                .GetCategoryInformationAsync(categorySegment.Select(c => c.ActionExtra), userIdBatchList);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                continue;

            var userVisitsDictionary = CreateResponseDictionary(response.Data);

            await TrySetMembershipAsync(userIdBatchList, userVisitsDictionary, categorySegment);

        }

    }

    private Dictionary<long, IEnumerable<MessageModel>> CreateResponseDictionary(IEnumerable<MessageModel> restResponse)
    {
        var responseGroupedByUserId = restResponse.GroupBy(c => long.Parse(c.UserId));

        var usersDictionary = new Dictionary<long, IEnumerable<MessageModel>>();
        foreach (var item in responseGroupedByUserId)
        {
            if (item.Any())
                usersDictionary.TryAdd(item.Key, item);
        }
        return usersDictionary;
    }

    private async Task<List<long>> ExtractUserIdsFromRequestCacheAsync()
    {
        var userIdBatchList = new List<long>();
        var count = 1;
        while (_setting.BatchSize > count)
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
        return userIdBatchList;
    }

    private async Task<IEnumerable<SegmentCache>> GetCategorySegmentsAsync()
    {
        return (await _segmentCache.GetListAsync())
             .Where(c => !string.IsNullOrEmpty(c.ActivityExtra)).ToList();
    }

    private async Task TrySetMembershipAsync(IEnumerable<long> userIdBatchList, Dictionary<long, IEnumerable<MessageModel>> userVisitsDictionary, IEnumerable<SegmentCache> categorySegment)
    {
        foreach (var item in userIdBatchList)
        {
            if (!userVisitsDictionary.TryGetValue(item, out IEnumerable<MessageModel> userVisits))
                continue;

            var userCategories = userVisits.Select(c => c.CategoryName);
            var segment = categorySegment.Where(c => userCategories.Contains(c.ActivityExtra));
            var finalSegmentsForRedis = new List<Membership>();
            if (segment == null)
                continue;

            finalSegmentsForRedis.AddRange(segment.Select(c => new Membership()
            {
                HostId = _setting.HostId ?? 0,
                SegmentId = c.Id
            }));

            await _responseRepositoy.SetMembership(ResponseUpdaterTypeEnum.Category, item,
                finalSegmentsForRedis, _setting.EmptyExpirationInMinutes, _setting.FullExpirationInMinutes);

        }
    }

}

