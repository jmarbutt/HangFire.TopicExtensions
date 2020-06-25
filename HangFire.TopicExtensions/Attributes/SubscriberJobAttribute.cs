using System;
using Hangfire.States;

namespace HangFire.TopicExtensions.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class SubscriberJobAttribute : Attribute
    {
        public SubscriberJobAttribute(string topic) : this(topic, EnqueuedState.DefaultQueue)
        {
        }


        public SubscriberJobAttribute(string topic, string queue)
        {
            if (string.IsNullOrEmpty(topic)) throw new ArgumentNullException(nameof(topic));

            if (string.IsNullOrEmpty(queue)) throw new ArgumentNullException(nameof(queue));


            Topic = topic;
            Queue = queue;
        }

        public string TopicJobId { get; set; }

        public string Queue { get; set; }


        public bool Enabled { get; set; } = true;

        public string Topic { get; set; }
    }
}