using MassTransit;
using Microsoft.Extensions.Logging;
using SocialService.Application.Interfaces;
using SocialService.Shared.Common.Events.Identity;
using System;
using System.Threading.Tasks;

namespace SocialService.Infrastructure.Messaging.Consumers
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
                "User deleted event received - UserId: {UserId}, DeletedAt: {DeletedAt}",
                message.UserId,
                message.DeletedAt);

            try
            {
                // User silindiğinde, o user'a ait tüm post'ları sil
                await _postRepository.DeletePostsByUserIdAsync(message.UserId);

                _logger.LogInformation(
                    "Successfully deleted all posts for user - UserId: {UserId}",
                    message.UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error occurred while deleting posts for user - UserId: {UserId}",
                    message.UserId);
                throw;
            }
        }
    }
}