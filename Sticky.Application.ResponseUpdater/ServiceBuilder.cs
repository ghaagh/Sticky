using Aerospike.Client;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Sticky.Application.ResponseUpdater.Services;
using Sticky.Domain.ResponseAggrigate;
using Sticky.Infrastructure.Cache;
using Sticky.Infrastructure.Cache.Models;
using Sticky.Infrastructure.Redis;

namespace Sticky.Application.ResponseUpdater;

internal class ServiceBuilder
{
    private readonly IServiceCollection _serviceCollection;
    const int AEROSPIKE_PORT = 3000;

    public ServiceBuilder(Setting setting)
    {
        _serviceCollection = new ServiceCollection();

        AddRedis(setting.Redis);

        switch (setting.ResponseUpdaterType)
        {
            case ResponseUpdaterTypeEnum.ProductAndPage:
                AddProductUpdaterService(setting.AerospikeClient, AEROSPIKE_PORT);
                break;
            case ResponseUpdaterTypeEnum.Category:
                AddCategoryUpdaterService();
                break;
            case ResponseUpdaterTypeEnum.SpecialSegment:
                AddQueryUpdaterService(setting.AerospikeClient, AEROSPIKE_PORT);
                break;
        }
    }
    public IServiceCollection BuildCollection() => _serviceCollection;

    private ServiceBuilder() { }

    private void AddQueryUpdaterService(string aerospikeClient, int port)
    {
        AddAeroSpikeClient(aerospikeClient, port);
        _serviceCollection.AddSingleton<IResponseUpdater, QueryResponseUpdater>();

    }

    private void AddRedis(string redisAddress)
    {
        ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(redisAddress);
        _serviceCollection.AddSingleton<IConnectionMultiplexer, ConnectionMultiplexer>(c => connectionMultiplexer);

        _serviceCollection.Configure<RedisConfig>(options =>
        {
            options.HostCache = new RedisItemConfig()
            {
                CacheType = CacheType.Hashset,
                DatabaseNumber = 0,
            };
            options.SegmentCache = new RedisItemConfig()
            {
                CacheType = CacheType.Hashset,
                DatabaseNumber = 0,
            };
            options.ProductCache = new RedisItemConfig()
            {
                CacheType = CacheType.Key,
                DatabaseNumber = 1,
            };
            options.PartnerCache = new RedisItemConfig()
            {
                CacheType = CacheType.Hashset,
                DatabaseNumber = 0
            };
        });

        _serviceCollection.AddSingleton<IMultipleCache<ProductCache>, RedisHashsetCache<ProductCache>>();
        _serviceCollection.AddSingleton<IMultipleCache<SegmentCache>, RedisHashsetCache<SegmentCache>>();
        _serviceCollection.AddSingleton<IMultipleCache<HostCache>, RedisHashsetCache<HostCache>>();
        _serviceCollection.AddSingleton<IResponseRepositoy, RedisResponseRepository>();
        _serviceCollection.AddSingleton<IRequestRepository, RedisRequestRepository>();
    }

    private void AddCategoryUpdaterService()
    {
        _serviceCollection.AddSingleton<IResponseUpdater, CategoryResponseUpdater>();
    }

    private void AddProductUpdaterService(string aerospikeClient, int port)
    {
        AddAeroSpikeClient(aerospikeClient, port);
        _serviceCollection.AddSingleton<IResponseUpdater, ProductResponseUpdater>();
    }

    private void AddAeroSpikeClient(string aerospikeClient, int port)
    {
        _serviceCollection.AddSingleton<IAsyncClient, AsyncClient>(c => new AsyncClient(aerospikeClient, port));
    }
}

