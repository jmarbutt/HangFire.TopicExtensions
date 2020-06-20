using System;
using System.Collections.Generic;
using Hangfire;

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
    }
}
