using MassTransit;
using Microsoft.Extensions.Logging;
using SocialService.Application.Interfaces;
using System;
using System.Threading.Tasks;
using CityDiscovery.VenueService.VenuesService.Shared.Common.Events.Venue;

namespace SocialService.Infrastructure.Messaging.Consumers
{
    public class VenueDeletedConsumer : IConsumer<VenueDeletedEvent>
    {
        private readonly IPostRepository _postRepository;
        private readonly ILogger<VenueDeletedConsumer> _logger;

        public VenueDeletedConsumer(IPostRepository postRepository, ILogger<VenueDeletedConsumer> logger)
        {
            _postRepository = postRepository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<VenueDeletedEvent> context)
        {
            var message = context.Message;

            _logger.LogInformation(
                "Venue deleted event received - VenueId: {VenueId}, DeletedAt: {DeletedAt}",
                message.VenueId,
                message.DeletedAt);

            try
            {
                // Venue silindiğinde, o venue'ye ait tüm post'ları sil
                await _postRepository.DeletePostsByVenueIdAsync(message.VenueId);

                _logger.LogInformation(
                    "Successfully deleted all posts for venue - VenueId: {VenueId}",
                    message.VenueId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error occurred while deleting posts for venue - VenueId: {VenueId}",
                    message.VenueId);
                throw;
            }
        }
    }
}