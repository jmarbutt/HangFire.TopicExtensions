using System;
using System.Collections.Generic;
using System.Reflection;
using Hangfire.States;
using HangFire.TopicExtensions.Attributes;
using HangFire.TopicExtensions.Interfaces;

namespace HangFire.TopicExtensions
{
    public class TopicJobBuilder : ITopicJobBuilder
    {
        private readonly ITopicJobRegistry _registry;

        public TopicJobBuilder() : this(new TopicJobRegistry())
        {
        }

        public TopicJobBuilder(ITopicJobRegistry registry)
        {
            _registry = registry;
        }


        public void Build(Func<IEnumerable<Type>> typesProvider)
        {
            if (typesProvider == null) throw new ArgumentNullException(nameof(typesProvider));

            foreach (var type in typesProvider())
            foreach (var method in type.GetTypeInfo().DeclaredMethods)
            {
                if (!method.IsDefined(typeof(SubscriberJobAttribute), false)) continue;

                var attribute = method.GetCustomAttribute<SubscriberJobAttribute>(false);

                if (attribute == null) continue;

                if (string.IsNullOrWhiteSpace(attribute.TopicJobId)) attribute.TopicJobId = method.GetTopicJobId();

                if (!attribute.Enabled)
                    //TopicJob.RemoveIfExists(attribute.TopicJobId);
                    continue;
                _registry.Register(
                    attribute.TopicJobId,
                    method,
                    attribute.Topic,
                    attribute.Queue ?? EnqueuedState.DefaultQueue);
            }
        }

        public void Build(Func<IEnumerable<TopicJobInfo>> topicJobInfoProvider)
        {
            foreach (var job in topicJobInfoProvider()) _registry.Register(job);
        }
    }
}