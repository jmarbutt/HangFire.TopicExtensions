using System;
using System.Collections.Generic;
using System.Text;
using Hangfire.States;

namespace HangFire.TopicExtensions.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    class SubscriberJobAttribute : Attribute
    {

		public string TopicJobId { get; set; }
	
		public string Queue { get; set; }

	
		public bool Enabled { get; set; } = true;
		
		public SubscriberJobAttribute(string topic) : this(topic, EnqueuedState.DefaultQueue) { }

		
		
		public SubscriberJobAttribute(string topic,  string queue)
		{
			if (string.IsNullOrEmpty(topic)) throw new ArgumentNullException(nameof(topic));
			
			if (string.IsNullOrEmpty(queue)) throw new ArgumentNullException(nameof(queue));

			
			Topic = topic;
			Queue = queue;
		}

        public string Topic { get; set; }
    }
}
