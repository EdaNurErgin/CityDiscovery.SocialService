using CityDiscovery.VenuesService.Shared.Common.Events.Venue;
using MassTransit;
using SocialService.Application.Interfaces;



namespace CityDiscovery.SocialService.API.Consumers
{
    public class VenueUpdatedConsumer : IConsumer<VenueUpdatedEvent>
    {
        private readonly IPostRepository _postRepository;
        private readonly ILogger<VenueUpdatedConsumer> _logger;

        public VenueUpdatedConsumer(IPostRepository postRepository, ILogger<VenueUpdatedConsumer> logger)
        {
            _postRepository = postRepository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<VenueUpdatedEvent> context)
        {
            var venueId = context.Message.VenueId;
            var newName = context.Message.Name;
            var newImage = context.Message.ImageUrl;

            _logger.LogInformation($"Mekan güncellemesi algılandı. VenueId: {venueId}, Yeni İsim: {newName}");

            // Repository'deki bulk update metodunu çağır
            await _postRepository.UpdateVenueDetailsAsync(venueId, newName, newImage);

            _logger.LogInformation($"VenueId: {venueId} için postlar güncellendi.");
        }
    }
}