using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Hangfire;
using HangFire.TopicExtensions.Attributes;
using HangFire.TopicExtensions.Interfaces;

namespace HangFire.TopicExtensions
{
    public class TopicPublisher : ITopicPublisher
    {
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly ITopicJobInfoStorage _jobInfoStorage;

        public TopicPublisher(IBackgroundJobClient backgroundJobClient, ITopicJobInfoStorage jobInfoStorage)
        {
            _backgroundJobClient = backgroundJobClient;
            _jobInfoStorage = jobInfoStorage;
        }

        public void EnqueueTopic(string topic, object context = null)
        {
           
            // Find Subscribers
            _backgroundJobClient.Enqueue(() => DispatchTopic(topic, context));

        }

        public void DispatchTopic(string topic, object context)
        {

            var allSubscribers = GetImplementationsOf<ISubscriber>();

            foreach (var type in allSubscribers)
            {

                var attributes = type.GetCustomAttributes<SubscriberJobAttribute>();

                var subscribed = attributes.Any(a=> a.Topic.ToLower() == topic);
                if (!subscribed) continue;

                var impl = (ISubscriber)Activator.CreateInstance(type);
                _backgroundJobClient.Enqueue(() => impl.Execute(context));

            }

        }

        private static IEnumerable<Type> GetImplementationsOf<TInterface>()
        {
            var interfaceType = typeof(TInterface);
            return AppDomain.CurrentDomain.GetAssemblies()
                .Select(assembly =>
                    assembly.GetTypes().Where(type => !type.IsInterface && interfaceType.IsAssignableFrom(type)))
                .SelectMany(implementation => implementation);
        }
    }


}