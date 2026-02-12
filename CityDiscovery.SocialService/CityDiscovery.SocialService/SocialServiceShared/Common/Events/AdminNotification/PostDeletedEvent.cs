using CityDiscovery.SocialService.SocialServiceShared.Common.Events; // IIntegrationEvent nerede ise orayı ekleyin

// DİKKAT: Namespace, AdminService'teki ile BİREBİR AYNI olmalı.
// RabbitMQ listenizde gördüğümüz namespace'i buraya yazıyoruz:
namespace CityDiscovery.AdminNotificationService.Shared.Common.Events.Social
{
    public class PostDeletedEvent : IIntegrationEvent
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public DateTime OccurredOn { get; private set; } = DateTime.UtcNow;

        // AdminService ne gönderiyorsa buraya onu yazmalısınız.
        // Genelde silme işlemi için sadece ID yeterlidir.
        public Guid PostId { get; set; }
    }
}