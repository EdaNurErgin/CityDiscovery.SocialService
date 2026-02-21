using IdentityService.Shared.MessageBus.Identity;
using MassTransit;
using SocialService.Application.Interfaces;



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

            
            _logger.LogInformation(
                "SocialService: Kullanıcı silme eventi alındı. User: {UserName} ({UserId}), Silinme Zamanı: {DeletedAt}",
                message.UserName, 
                message.UserId,
                message.DeletedAtUtc);

            try
            {
                // User silindiğinde, o user'a ait tüm post'ları sil
                
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

                
                throw;
            }
        }
    }
}