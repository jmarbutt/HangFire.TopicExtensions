using System;
using System.Collections.Generic;
using Hangfire;
using HangFire.TopicExtensions.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HangFire.TopicExtensions
{
    public static class HangFireExtensions
    {
        public static IGlobalConfiguration UseTopics(this IGlobalConfiguration configuration, params Type[] types)
        {
            return UseTopics(configuration, () => types);
        }

        public static IGlobalConfiguration UseTopics(this IGlobalConfiguration configuration,
            Func<IEnumerable<Type>> typesProvider)
        {
            if (typesProvider == null) throw new ArgumentNullException(nameof(typesProvider));


            return configuration;
        }

        public static void AddTopicServices(this IServiceCollection services)
        {
            services.AddScoped<ITopicPublisher, TopicPublisher>();
        }
    }
}