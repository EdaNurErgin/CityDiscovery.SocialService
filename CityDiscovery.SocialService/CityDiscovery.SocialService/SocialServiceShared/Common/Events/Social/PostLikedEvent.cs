using System;

namespace CityDiscovery.SocialService.SocialServiceShared.Common.Events.Social
{
    public class PostLikedEvent
    {
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
        public DateTime LikedAt { get; set; }

        public PostLikedEvent(Guid postId, Guid userId, DateTime likedAt)
        {
            PostId = postId;
            UserId = userId;
            LikedAt = likedAt;
        }
    }
}