using System;
using System.Collections.Generic;
using System.Linq;
using HangFire.TopicExtensions.Configuration;
using HangFire.TopicExtensions.Interfaces;

namespace HangFire.TopicExtensions
{
    public class TopicJob
    {
        public static void AddOrUpdate(params Type[] types)
        {
            AddOrUpdate(() => types);
        }


        public static void AddOrUpdate(Func<IEnumerable<Type>> typesProvider)
        {
            if (typesProvider == null) throw new ArgumentNullException(nameof(typesProvider));

            ITopicJobBuilder builder = new TopicJobBuilder();

            builder.Build(typesProvider);
        }


        public static void AddOrUpdate(IConfigurationProvider provider)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));

            ITopicJobBuilder builder = new TopicJobBuilder();

            AddOrUpdate(provider.Load());
        }


        public static void AddOrUpdate(IEnumerable<TopicJobInfo> topicJobInfos)
        {
            if (topicJobInfos == null) throw new ArgumentNullException(nameof(topicJobInfos));

            ITopicJobBuilder builder = new TopicJobBuilder();

            builder.Build(() => topicJobInfos);
        }

        public static void AddOrUpdate(params TopicJobInfo[] topicJobInfos)
        {
            if (topicJobInfos == null) throw new ArgumentNullException(nameof(topicJobInfos));

            AddOrUpdate(topicJobInfos.AsEnumerable());
        }
    }
}