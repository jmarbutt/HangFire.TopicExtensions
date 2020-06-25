using System.Threading.Tasks;
using HangFire.TopicExtensions.Attributes;
using HangFire.TopicExtensions.Interfaces;

namespace HangFirePubSub.Sample
{
    [SubscriberJob("topic1")]
    public class SampleSubscriber2 : ISubscriber
    {
        public async Task Execute(object context)
        {
            
        }
    }
}