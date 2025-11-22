using MassTransit;
using Microsoft.Extensions.Logging;
using SocialService.Shared.Common.Events.Identity;

namespace CityDiscovery.SocialService.SocialService.Infrastructure.Messaging.Consumers
{
    public class UserLoggedInConsumer : IConsumer<UserLoggedInEvent>
    {
        private readonly ILogger<UserLoggedInConsumer> _logger;

        public UserLoggedInConsumer(ILogger<UserLoggedInConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<UserLoggedInEvent> context)
        {
            var message = context.Message;

            _logger.LogInformation(
                "User logged in event received - UserId: {UserId}, UserName: {UserName}, Email: {Email}, Role: {Role}, DeviceId: {DeviceId}, LoggedInAt: {LoggedInAt}",
                message.UserId,
                message.UserName,
                message.Email,
                message.Role,
                message.DeviceId,
                message.LoggedInAtUtc);

            // TODO: SocialService'in yapması gereken işlemler
            // Örnekler:
            // - Kullanıcı için feed hazırla
            // - Takip listesi oluştur
            // - Kullanıcı aktivite kaydı oluştur
            // - Cache'i güncelle
            // - vb.

            await Task.CompletedTask;
        }
    }
}

