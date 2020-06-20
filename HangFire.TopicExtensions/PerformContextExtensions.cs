using System;
using System.Collections.Generic;
using Hangfire.Common;
using Hangfire.Server;

namespace HangFire.TopicExtensions
{
    public static class PerformContextExtensions
    {
        public static object GetJobData(this PerformContext context, string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            var jobData = GetJobData(context);

            if (jobData == null) return null;

            return jobData.ContainsKey(name) ? jobData[name] : null;
        }


        public static T GetJobData<T>(this PerformContext context, string name)
        {
            var o = GetJobData(context, name);

            var json = JobHelper.ToJson(o);

            return JobHelper.FromJson<T>(json);
        }


        public static IDictionary<string, object> GetJobData(this PerformContext context)
        {
            using var storage = new TopicJobInfoStorage(context.Connection);
            return storage.FindByJobId(context.BackgroundJob.Id)?.JobData;
        }


        public static void SetJobData(this PerformContext context, string name, object value)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            SetJobData(context, new Dictionary<string, object> {[name] = value});
        }


        public static void SetJobData(this PerformContext context, IDictionary<string, object> jobData)
        {
            if (jobData == null) throw new ArgumentNullException(nameof(jobData));

            using var storage = new TopicJobInfoStorage(context.Connection);
            var recurringJobInfo = storage.FindByJobId(context.BackgroundJob.Id);

            recurringJobInfo.JobData ??= new Dictionary<string, object>();

            foreach (var kv in jobData)
                recurringJobInfo.JobData[kv.Key] = kv.Value;

            storage.SetJobData(recurringJobInfo);
        }
    }
}