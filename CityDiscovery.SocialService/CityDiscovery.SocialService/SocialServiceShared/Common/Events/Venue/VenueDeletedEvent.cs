using System;
using CityDiscovery.SocialService.SocialServiceShared.Common.Events;

// Namespace yap?s?n? projenizdeki mevcut yap?ya sad?k kalarak korudum
namespace CityDiscovery.VenueService.VenuesService.Shared.Common.Events.Venue
{
    public class VenueDeletedEvent : IIntegrationEvent
    {
        // --- IIntegrationEvent'ten gelen zorunlu alanlar ---
        public Guid Id { get; private set; } = Guid.NewGuid();
        public DateTime OccurredOn { get; private set; } = DateTime.UtcNow;
        // ----------------------------------------------------

        public Guid VenueId { get; set; }
        public DateTime DeletedAt { get; set; }
        public string VenueName { get; set; }

        // Parametresiz constructor (MassTransit/Serializer için ?ART)
        public VenueDeletedEvent() { }

        // Kolay olu?turmak için constructor
        public VenueDeletedEvent(Guid venueId, string venueName, DateTime deletedAt)
        {
            VenueId = venueId;
            VenueName = venueName;
            DeletedAt = deletedAt;
        }
    }
}