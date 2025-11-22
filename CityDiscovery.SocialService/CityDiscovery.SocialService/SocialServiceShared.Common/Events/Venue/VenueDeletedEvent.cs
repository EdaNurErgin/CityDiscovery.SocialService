using System;

namespace SocialService.Shared.Common.Events.Venue
{
    public class VenueDeletedEvent
    {
        public Guid VenueId { get; set; }
        public DateTime DeletedAt { get; set; }

        public VenueDeletedEvent(Guid venueId, DateTime deletedAt)
        {
            VenueId = venueId;
            DeletedAt = deletedAt;
        }
    }
}

