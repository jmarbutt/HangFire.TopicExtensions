using System.Collections.Generic;
using Hangfire;

namespace HangFire.TopicExtensions
{
    public interface ITopicJobInfoStorage
    {

        IEnumerable<TopicJobInfo> FindAll();

        TopicJobInfo FindByJobId(string jobId);


        TopicJobInfo FindByTopicJobId(string topicJobId);


        void SetJobData(TopicJobInfo topicJobInfo);
    }
}