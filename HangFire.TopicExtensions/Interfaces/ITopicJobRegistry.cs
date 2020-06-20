using System.Reflection;

namespace HangFire.TopicExtensions.Interfaces
{
    public interface ITopicJobRegistry
    {
        void Register(string topicJobId, MethodInfo method, string topic, string queue);

        void Register(TopicJobInfo topicJobInfo);
    }
}