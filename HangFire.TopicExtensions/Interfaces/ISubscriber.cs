using System.Threading.Tasks;

namespace HangFire.TopicExtensions.Interfaces
{
    public interface ISubscriber
    {
        Task Execute(object context);
    }
}