using System.Threading;
using System.Threading.Tasks;

namespace SocialService.Application.Interfaces
{
    public interface IMessageBus
    {
        Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class;
    }
}