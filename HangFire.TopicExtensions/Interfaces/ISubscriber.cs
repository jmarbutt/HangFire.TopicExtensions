using System;

using Hangfire.Server;

namespace HangFire.TopicExtensions.Interfaces
{
    public interface ISubscriber
    {
        void Execute(object context);
    }
}
