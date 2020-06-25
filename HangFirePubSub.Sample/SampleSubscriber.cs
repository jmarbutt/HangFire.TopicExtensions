using System.Threading.Tasks;
using Hangfire.Server;
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
        public async Task Execute(object context)
        {
            
        }
    }
}