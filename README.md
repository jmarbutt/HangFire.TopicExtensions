# HangFire.TopicExtensions




## Setup 

Place this snipped in your `ConfigureServices`

```c#
 services
    .AddHangfire(x =>
        x
            .UseSqlServerStorage(
                "<connection>")
            .UseTopics()
    );

services.AddHangfireServer();
services.AddTopicServices();
```


## Example Sending Topic

```c#
topicPublisher.EnqueueTopic("topic1");

topicPublisher.EnqueueTopic("topic1", someParams);
```


## Example Subscriber

```c#
    [SubscriberJob("topic1")]
    public class SampleSubscriber : ISubscriber
    {
        private readonly IConfiguration _config;

        public SampleSubscriber(IConfiguration config)
        {
            _config = config;
        }
        public void Execute(object context)
        {
            
        }
    }

```