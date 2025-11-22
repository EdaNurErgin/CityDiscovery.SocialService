using MassTransit;
using SocialService.Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace CityDiscovery.SocialService.SocialService.Infrastructure.Messaging
{
    public class MessageBus : IMessageBus
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public MessageBus(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class
        {
            await _publishEndpoint.Publish(message, cancellationToken);
        }
    }
}
