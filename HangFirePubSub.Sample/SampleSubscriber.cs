﻿using Hangfire.Server;
using HangFire.TopicExtensions.Attributes;
using HangFire.TopicExtensions.Interfaces;
using Microsoft.Extensions.Configuration;

namespace HangFirePubSub.Sample
{
    [SubscriberJob("topic1")]
    public class SampleSubscriber : ISubscriber
    {
        private readonly IConfiguration _config;

        public SampleSubscriber(IConfiguration config)
        {
            _config = config;
        }
        public void Execute(object context)
        {
            
        }
    }

    [SubscriberJob("topic1")]
    public class SampleSubscriber2 : ISubscriber
    {
        public void Execute(object context)
        {
            
        }
    }

    [SubscriberJob("topic2")]
    public class SampleSubscriber3 : ISubscriber
    {
        public void Execute(object context)
        {
            
        }
    }
}