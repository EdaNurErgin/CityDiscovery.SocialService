using CityDiscovery.SocialService.SocialServiceShared.Common.Events;


namespace CityDiscovery.VenuesService.Shared.Common.Events.Venue
{
    public class VenueUpdatedEvent : IIntegrationEvent
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public DateTime OccurredOn { get; private set; } = DateTime.UtcNow;

        public Guid VenueId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}