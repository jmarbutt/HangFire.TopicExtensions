namespace HangFire.TopicExtensions.Interfaces
{
    public interface ISubscriber
    {
        void Execute(object context);
    }
}