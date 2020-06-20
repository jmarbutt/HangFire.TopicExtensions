using System;
using System.Reflection;
using HangFire.TopicExtensions.Interfaces;

namespace HangFire.TopicExtensions
{
    public class TopicJobRegistry : ITopicJobRegistry
    {
        public void Register(string topicJobId, MethodInfo method, string topic, string queue)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (topic == null) throw new ArgumentNullException(nameof(topic));
            if (topicJobId == null) throw new ArgumentNullException(nameof(topicJobId));
            if (queue == null) throw new ArgumentNullException(nameof(queue));


            Register(new TopicJobInfo()
            {
                TopicJobId =  topicJobId,
                Method = method,
                Topic = topic,
                Queue = queue
            });
          
        }

        public void Register(TopicJobInfo topicJobInfo)
        {
            if (topicJobInfo == null) throw new ArgumentNullException(nameof(topicJobInfo));

            using var storage = new TopicJobInfoStorage();
            storage.SetJobData(topicJobInfo);
        }
    }
}