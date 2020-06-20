using System;
using System.Linq.Expressions;
using System.Reflection;
using HangFire.TopicExtensions.Interfaces;

namespace HangFire.TopicExtensions
{
    public class TopicJobRegistry : ITopicJobRegistry
    {
        public void Register(string topicJobId, MethodInfo method, string topic, string queue)
        {
            if (topicJobId == null) throw new ArgumentNullException(nameof(topicJobId));
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (topic == null) throw new ArgumentNullException(nameof(topic));

            if (queue == null) throw new ArgumentNullException(nameof(queue));

            var parameters = method.GetParameters();

            var args = new Expression[parameters.Length];

            for (var i = 0; i < parameters.Length; i++) args[i] = Expression.Default(parameters[i].ParameterType);

            var x = Expression.Parameter(method.DeclaringType, "x");

            var methodCall = method.IsStatic ? Expression.Call(method, args) : Expression.Call(x, method, args);

           
            var addOrUpdate = Expression.Call(
                typeof(TopicJob),
                nameof(TopicJob.AddOrUpdate),
                new Type[] { method.DeclaringType },
                new Expression[]
                {
                    Expression.Constant(topicJobId),
                    Expression.Lambda(methodCall, x),
                    Expression.Constant(topic),
                    Expression.Constant(queue)
                });

            Expression.Lambda(addOrUpdate).Compile().DynamicInvoke();
        }

        public void Register(TopicJobInfo topicJobInfo)
        {
            if (topicJobInfo == null) throw new ArgumentNullException(nameof(topicJobInfo));

            Register(topicJobInfo.TopicJobId, topicJobInfo.Method, topicJobInfo.Topic, topicJobInfo.Queue);

            using var storage = new TopicJobInfoStorage();
            storage.SetJobData(topicJobInfo);
        }
    }
}