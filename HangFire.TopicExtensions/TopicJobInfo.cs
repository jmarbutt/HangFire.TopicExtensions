using System.Collections.Generic;
using System.Reflection;

namespace HangFire.TopicExtensions
{
    public class TopicJobInfo
    {
   
        public string TopicJobId { get; set; }


        public string Queue { get; set; }


        public MethodInfo Method { get; set; }

        public IDictionary<string, object> JobData { get; set; }


        public bool Enable { get; set; }

        public string Topic { get; set; }
    }
}