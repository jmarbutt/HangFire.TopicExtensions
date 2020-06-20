using System;
using System.Collections.Generic;
using Hangfire;

namespace HangFire.TopicExtensions
{
    public static class HangFireExtensions
    {
        public static IGlobalConfiguration UseTopics(this IGlobalConfiguration configuration)
        {


            return configuration;
        }


    }
}
