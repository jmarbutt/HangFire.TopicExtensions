﻿namespace HangFire.TopicExtensions.Interfaces
{
    public interface ITopicPublisher
    {
        void EnqueueTopic(string topic, object context = null);
    }
}