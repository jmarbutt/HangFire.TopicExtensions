using System.Collections.Generic;

namespace HangFire.TopicExtensions.Configuration
{
    public interface IConfigurationProvider
    {
        IEnumerable<TopicJobInfo> Load();
    }
}