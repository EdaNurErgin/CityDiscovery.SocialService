using System;

namespace SocialService.Shared.Common.Events.Identity
{
    public class UserDeletedEvent
    {
        public Guid UserId { get; set; }
        public DateTime DeletedAt { get; set; }

        public UserDeletedEvent(Guid userId, DateTime deletedAt)
        {
            UserId = userId;
            DeletedAt = deletedAt;
        }
    }
}

