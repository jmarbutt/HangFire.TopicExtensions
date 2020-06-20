using System;
using System.Collections.Generic;
using Hangfire;
using HangFire.TopicExtensions.Configuration;
using HangFire.TopicExtensions.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HangFire.TopicExtensions
{
    public static class HangFireExtensions
    {
        public static IGlobalConfiguration UseTopics(this IGlobalConfiguration configuration,params Type[] types)
        {
            return UseTopics(configuration, () => types);
        }

        public static IGlobalConfiguration UseTopics(this IGlobalConfiguration configuration, Func<IEnumerable<Type>> typesProvider)
        {
            if (typesProvider == null) throw new ArgumentNullException(nameof(typesProvider));

            TopicJob.AddOrUpdate(typesProvider);
            
            return configuration;
        }

        public static IGlobalConfiguration UseTopics(this IGlobalConfiguration configuration, IConfigurationProvider provider)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));

            TopicJob.AddOrUpdate(provider);

            return configuration;
        }


        public static void AddTopicServices(this IServiceCollection services)
        {
            services.AddScoped<ITopicPublisher,TopicPublisher>();
            services.AddScoped<ITopicJobInfoStorage,TopicJobInfoStorage>();
        }
    }
}
