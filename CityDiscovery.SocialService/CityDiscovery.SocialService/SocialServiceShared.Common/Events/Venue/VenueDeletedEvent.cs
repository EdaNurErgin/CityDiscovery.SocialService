using System;

// Namespace VenueService'deki ile birebir ayn? olmal?:
namespace CityDiscovery.VenueService.VenuesService.Shared.Common.Events.Venue
{
    public class VenueDeletedEvent
    {
        public Guid VenueId { get; set; }
        public DateTime DeletedAt { get; set; }
        // VenueService taraf?nda VenueName de var, buraya da eklemeniz iyi olur (serile?tirme hatas? olmamas? için)
        public string VenueName { get; set; }

        // Parametresiz constructor (Serializer için gereklidir)
        public VenueDeletedEvent() { }

        public VenueDeletedEvent(Guid venueId, string venueName, DateTime deletedAt)
        {
            VenueId = venueId;
            VenueName = venueName;
            DeletedAt = deletedAt;
        }
    }
}