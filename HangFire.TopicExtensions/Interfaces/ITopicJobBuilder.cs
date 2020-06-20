using System;
using System.Collections.Generic;
using System.Text;

namespace HangFire.TopicExtensions.Interfaces
{
    interface ITopicJobBuilder 
    {
        void Build(Func<IEnumerable<Type>> typesProvider);

        void Build(Func<IEnumerable<TopicJobInfo>> recurringJobInfoProvider);
    }
}
