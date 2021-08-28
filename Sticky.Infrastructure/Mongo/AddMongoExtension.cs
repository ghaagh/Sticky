using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Sticky.Domain.CookieSyncing;

namespace Sticky.Infrastructure.Mongo
{
    public static class AddMongoExtension
    {
        public static IServiceCollection AddMongo(this IServiceCollection services, MongoConfig config)
        {
            var mongoClient = new MongoClient(config.ConnectionString);
            services.AddSingleton<IMongoClient, MongoClient>(c => mongoClient);
            services.AddSingleton<ICookieSyncRepository, MongoCookieSyncRepository>();
            return services;
        }
    }
}
