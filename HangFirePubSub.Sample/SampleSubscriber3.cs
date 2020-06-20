using HangFire.TopicExtensions.Attributes;
using HangFire.TopicExtensions.Interfaces;

namespace HangFirePubSub.Sample
{
    [SubscriberJob("topic2")]
    public class SampleSubscriber3 : ISubscriber
    {
        public void Execute(object context)
        {
            
        }
    }
}