using IdentityService.Shared.MessageBus.Identity; 
using MassTransit;
using SocialService.Application.Interfaces;

namespace SocialService.Infrastructure.Messaging.Consumers
{
    public class UserUpdatedConsumer : IConsumer<UserUpdatedEvent>
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;

        public UserUpdatedConsumer(
            IPostRepository postRepository,
            ICommentRepository commentRepository)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
        }

        public async Task Consume(ConsumeContext<UserUpdatedEvent> context)
        {
            var message = context.Message;

            // 1. Kullanıcının tüm gönderilerindeki (Posts) adını ve profil fotoğrafını güncelle
            await _postRepository.UpdateAuthorDetailsAsync(
                message.UserId,
                message.NewUserName,
                message.NewAvatarUrl);

            // 2. Kullanıcının tüm yorumlarındaki (Comments) adını ve profil fotoğrafını güncelle
            await _commentRepository.UpdateAuthorDetailsAsync(
                message.UserId,
                message.NewUserName,
                message.NewAvatarUrl);
        }
    }
}