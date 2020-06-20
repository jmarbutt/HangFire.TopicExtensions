using System;
using System.Collections.Generic;
using Hangfire;
using Hangfire.Common;
using Hangfire.Storage;

namespace HangFire.TopicExtensions
{
    public class TopicJobInfoStorage : ITopicJobInfoStorage, IDisposable
    {
        private static readonly TimeSpan LockTimeout = TimeSpan.FromMinutes(1);

        private readonly IStorageConnection _connection;


        public TopicJobInfoStorage() : this(JobStorage.Current.GetConnection())
        {
        }

        public TopicJobInfoStorage(IStorageConnection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        /// <summary>
        ///     Disposes storage connection.
        /// </summary>
        public void Dispose()
        {
            _connection?.Dispose();
        }

        public IEnumerable<TopicJobInfo> FindAll()
        {
            var topicJobIds = _connection.GetAllItemsFromSet("topic-jobs");

            foreach (var topicJobId in topicJobIds)
            {
                var topicJob = _connection.GetAllEntriesFromHash($"topic-job:{topicJobId}");

                if (topicJob == null) continue;

                yield return InternalFind(topicJobId, topicJob);
            }
        }

        public TopicJobInfo FindByJobId(string jobId)
        {
            if (string.IsNullOrEmpty(jobId)) throw new ArgumentNullException(nameof(jobId));

            var paramValue = _connection.GetJobParameter(jobId, "TopicJobId");

            if (string.IsNullOrEmpty(paramValue))
                throw new Exception($"There is not TopicJobId with associated BackgroundJob Id:{jobId}");

            var topicJobId = JobHelper.FromJson<string>(paramValue);

            return FindByTopicJobId(topicJobId);
        }

        public TopicJobInfo FindByTopicJobId(string topicJobId)
        {
            if (string.IsNullOrEmpty(topicJobId)) throw new ArgumentNullException(nameof(topicJobId));

            var topicJob = _connection.GetAllEntriesFromHash($"topic-job:{topicJobId}");

            return topicJob == null ? null : InternalFind(topicJobId, topicJob);
        }


        public void SetJobData(TopicJobInfo topicJobInfo)
        {
            if (topicJobInfo == null) throw new ArgumentNullException(nameof(topicJobInfo));

            if (topicJobInfo.JobData == null || topicJobInfo.JobData.Count == 0) return;

            using (_connection.AcquireDistributedLock($"topicjobextensions-jobdata:{topicJobInfo.TopicJobId}",
                LockTimeout))
            {
                var changedFields = new Dictionary<string, string>
                {
                    [nameof(TopicJobInfo.Enable)] = JobHelper.ToJson(topicJobInfo.Enable),
                    [nameof(TopicJobInfo.JobData)] = JobHelper.ToJson(topicJobInfo.JobData)
                };

                _connection.SetRangeInHash($"topic-job:{topicJobInfo.TopicJobId}", changedFields);
            }
        }

        private TopicJobInfo InternalFind(string topicJobId, Dictionary<string, string> topicJob)
        {
            if (string.IsNullOrEmpty(topicJobId)) throw new ArgumentNullException(nameof(topicJobId));
            if (topicJob == null) throw new ArgumentNullException(nameof(topicJob));

            var serializedJob = JobHelper.FromJson<InvocationData>(topicJob["Job"]);
            var job = serializedJob.Deserialize();

            return new TopicJobInfo
            {
                TopicJobId = topicJobId,
                Topic = topicJob["Topic"],
                Queue = topicJob["Queue"],
                Method = job.Method,
                Enable = topicJob.ContainsKey(nameof(TopicJobInfo.Enable))
                    ? JobHelper.FromJson<bool>(topicJob[nameof(TopicJobInfo.Enable)])
                    : true,
                JobData = topicJob.ContainsKey(nameof(TopicJobInfo.JobData))
                    ? JobHelper.FromJson<Dictionary<string, object>>(topicJob[nameof(TopicJobInfo.JobData)])
                    : null
            };
        }
    }
}