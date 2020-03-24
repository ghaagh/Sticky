using StackExchange.Redis;
using Sticky.Models.Etc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sticky.Repositories.Common.Implementions
{
    public class RedisCache:IRedisCache
    {
        private static readonly Lazy<ConnectionMultiplexer> LazyConnection;
        public IDatabase GetDatabase(RedisDatabases redisDatabase)
        {
            var redis = LazyConnection.Value.GetDatabase((int)redisDatabase);
            return redis;

        }
#pragma warning disable CA1810 // Initialize reference type static fields inline
        static RedisCache()
#pragma warning restore CA1810 // Initialize reference type static fields inline
        {
            LazyConnection = new Lazy<ConnectionMultiplexer>();
            ConfigurationOptions redisOption = new ConfigurationOptions
            {
                ConnectTimeout = 500,
                AbortOnConnectFail = true,
                EndPoints = { "localhost:6379" }
            };
            var t1 = ConnectionMultiplexer.Connect(redisOption);
            t1.PreserveAsyncOrder = false;
            LazyConnection = new Lazy<ConnectionMultiplexer>(() => t1);
        }
    }
}

