using MediatR;


namespace CityDiscovery.SocialService.SocialServiceShared.Common.Events.Social
{
    public class PostCreatedEvent : INotification
    {
        public Guid PostId { get; set; }
        public Guid VenueId { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }

        public PostCreatedEvent(Guid postId, Guid venueId, Guid userId, string content, DateTime createdDate)
        {
            PostId = postId;
            VenueId = venueId;
            UserId = userId;
            Content = content;
            CreatedDate = createdDate;
        }
    }
}