using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Hangfire;
using HangFire.TopicExtensions.Attributes;
using HangFire.TopicExtensions.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HangFire.TopicExtensions
{
    public class TopicPublisher : ITopicPublisher
    {
        private readonly IServiceProvider _serviceProvider;

        public TopicPublisher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            BuildSubscribers();
        }

        public List<Subscription> Subscriptions { get; set; }

        public void EnqueueTopic(string topic, object context = null)
        {
            // Find Subscribers
            BackgroundJob.Enqueue(() => DispatchTopic(topic, context));
        }

        private void BuildSubscribers()
        {
            var allTypes = GetImplementationsOf<ISubscriber>();
            Subscriptions = new List<Subscription>();

            foreach (var type in allTypes)
            {
                var attributes = type.GetCustomAttributes<SubscriberJobAttribute>();

                var subscriberJobAttributes = attributes.ToList();
                if (!subscriberJobAttributes.Any()) continue;


                var impl = (ISubscriber) ActivatorUtilities.CreateInstance(_serviceProvider, type);

                foreach (var subscriberJobAttribute in subscriberJobAttributes)
                {
                    var subscription = new Subscription
                    {
                        Topic = subscriberJobAttribute.Topic,
                        Subscriber = impl
                    };

                    Subscriptions.Add(subscription);
                }
            }
        }

        public void DispatchTopic(string topic, object context)
        {
            foreach (var subscription in Subscriptions.Where(s => s.Topic == topic))
                BackgroundJob.Enqueue(() => subscription.Subscriber.Execute(context));
        }

        private static IEnumerable<Type> GetImplementationsOf<TInterface>()
        {
            var interfaceType = typeof(TInterface);
            return AppDomain.CurrentDomain.GetAssemblies()
                .Select(assembly =>
                    assembly.GetTypes().Where(type => !type.IsInterface && interfaceType.IsAssignableFrom(type)))
                .SelectMany(implementation => implementation);
        }

        public class Subscription
        {
            public string Topic { get; set; }
            public ISubscriber Subscriber { get; set; }
        }
    }
}