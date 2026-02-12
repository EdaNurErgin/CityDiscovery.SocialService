using MassTransit;
using SocialService.Application.Interfaces;
// Namespace'in AdminNotificationService'teki Event ile AYNI olduğundan emin olun!
using CityDiscovery.AdminNotificationService.Shared.Common.Events.Social;

namespace SocialService.API.Consumers
{
    public class PostDeletedConsumer : IConsumer<PostDeletedEvent>
    {
        private readonly IPostRepository _postRepository;
        // private readonly ILogger<PostDeletedConsumer> _logger;

        public PostDeletedConsumer(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task Consume(ConsumeContext<PostDeletedEvent> context)
        {
            var postId = context.Message.PostId;

            // 1. Postu veritabanından bul ve sil
            // Repository'nizde DeleteAsync veya SoftDelete metodu olmalı
            await _postRepository.DeleteAsync(postId);

            Console.WriteLine($"Admin tarafından silinen post (Id: {postId}) veritabanından kaldırıldı.");
        }
    }
}