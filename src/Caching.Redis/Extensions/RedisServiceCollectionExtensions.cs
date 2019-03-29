using System;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.DependencyInjection;
using Patterson.Caching.Redis.Interfaces;

namespace Patterson.Caching.Redis.Extensions
{
    public static class RedisServiceCollectionExtensions
    {
        public static IServiceCollection AddRedisCache(this IServiceCollection serviceCollection, Action<RedisCacheOptions> setupAction)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            serviceCollection.AddOptions();
            serviceCollection.Configure(setupAction);
            serviceCollection.Add(ServiceDescriptor.Singleton<ICache, StackExchangeRedisCache>());

            return serviceCollection;
        }
    }
}
