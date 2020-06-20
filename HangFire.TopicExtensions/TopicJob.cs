using System;
using System.Collections.Generic;
using HangFire.TopicExtensions.Interfaces;

namespace HangFire.TopicExtensions
{
    public class TopicJob
    {
   
        public static void AddOrUpdate(params Type[] types)
        {
            AddOrUpdate(() => types);
        }

        public static void AddOrUpdate(Func<IEnumerable<Type>> typesProvider)
        {
            if (typesProvider == null) throw new ArgumentNullException(nameof(typesProvider));

            ITopicJobBuilder builder = new TopicJobBuilder();

            builder.Build(typesProvider);
        }
    }
}