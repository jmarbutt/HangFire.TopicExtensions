using Hangfire.Server;
using HangFire.TopicExtensions.Attributes;

namespace HangFirePubSub.Sample
{
    public class SampleSubscriber
    {
        [SubscriberJob("topic1")]
        public void Subscriber1(PerformContext context)
        {
        }

        [SubscriberJob("topic1")]
        public void Subscriber2(PerformContext context)
        {
        }

        [SubscriberJob("topic2")]
        public void Subscriber3(PerformContext context)
        {
        }
    }
}