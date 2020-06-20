using Hangfire;
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

        public void EnqueueTopic(string topic)
        {
            
        }
    }

}