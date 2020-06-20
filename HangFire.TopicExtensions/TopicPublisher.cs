﻿using System;
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
        private readonly IBackgroundJobClient _backgroundJobClient;
        
        private readonly IServiceProvider _serviceProvider;

        public TopicPublisher(
            IBackgroundJobClient backgroundJobClient, 
            
            IServiceProvider serviceProvider)
        {
            _backgroundJobClient = backgroundJobClient;
            
            _serviceProvider = serviceProvider;
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
                
                var impl = (ISubscriber)ActivatorUtilities.CreateInstance(_serviceProvider,type);
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