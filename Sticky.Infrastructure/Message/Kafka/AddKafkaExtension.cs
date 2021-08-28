using Microsoft.Extensions.DependencyInjection;
using System;

namespace Sticky.Infrastructure.Message.Kafka.Extensions
{
    public static class AddKafkaExtension
    {
        public static IServiceCollection AddKafka(this IServiceCollection services, Action<KafkaConfig> configuration)
        {
            services.Configure(configuration);
            services.AddSingleton<IMessage, KafkaMessager>();
            return services;
        }
    }
}
