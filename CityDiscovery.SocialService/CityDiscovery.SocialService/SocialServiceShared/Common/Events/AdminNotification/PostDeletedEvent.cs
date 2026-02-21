using CityDiscovery.SocialService.SocialServiceShared.Common.Events; 


namespace CityDiscovery.AdminNotificationService.Shared.Common.Events.Social
{
    public class PostDeletedEvent : IIntegrationEvent
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public DateTime OccurredOn { get; private set; } = DateTime.UtcNow;

        public Guid PostId { get; set; }
    }
}