//using System;

//namespace IdentityService.Shared.MessageBus.Identity
//{
//    public class UserDeletedEvent
//    {
//        public Guid UserId { get; set; }
//        public DateTime DeletedAt { get; set; }

//        public UserDeletedEvent(Guid userId, DateTime deletedAt)
//        {
//            UserId = userId;
//            DeletedAt = deletedAt;
//        }
//    }
//}

using System;

// Namespace, Identity Service'teki ile B?REB?R AYNI olmal?
namespace IdentityService.Shared.MessageBus.Identity
{
    // Class de?il Record kullan?yoruz (Identity Service ile uyum için)
    // Parametreler ve s?ralar? Identity Service ile ayn? olmal?
    public record UserDeletedEvent(
        Guid UserId,
        string UserName,
        string Email,
        string Role,
        DateTime DeletedAtUtc
    );
}