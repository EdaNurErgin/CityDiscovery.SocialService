using IdentityService.Shared.MessageBus.Identity;
using MassTransit;
using Microsoft.Extensions.Logging;
using SocialService.Application.Interfaces;
using System;
using System.Threading.Tasks;

namespace CityDiscovery.SocialService.Infrastructure.Messaging.Consumers
{
    public class UserDeletedConsumer : IConsumer<UserDeletedEvent>
    {
        private readonly IPostRepository _postRepository;
        private readonly ILogger<UserDeletedConsumer> _logger;

        public UserDeletedConsumer(IPostRepository postRepository, ILogger<UserDeletedConsumer> logger)
        {
            _postRepository = postRepository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<UserDeletedEvent> context)
        {
            var message = context.Message;

            // DÜZELTME 1: Property ismi 'DeletedAt' yerine 'DeletedAtUtc' olmalı
            _logger.LogInformation(
                "SocialService: Kullanıcı silme eventi alındı. User: {UserName} ({UserId}), Silinme Zamanı: {DeletedAt}",
                message.UserName, // Artık UserName bilgisine de erişebilirsiniz
                message.UserId,
                message.DeletedAtUtc);

            try
            {
                // User silindiğinde, o user'a ait tüm post'ları sil
                // DÜZELTME 2: Bu metodun IPostRepository'de tanımlı olduğundan emin olun
                await _postRepository.DeletePostsByUserIdAsync(message.UserId);

                _logger.LogInformation(
                    "SocialService: Kullanıcıya ait tüm postlar başarıyla silindi - UserId: {UserId}",
                    message.UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "SocialService: Post silme işlemi sırasında hata oluştu - UserId: {UserId}",
                    message.UserId);

                // Hata fırlatarak mesajın retry kuyruğuna düşmesini sağlayabilirsiniz
                throw;
            }
        }
    }
}