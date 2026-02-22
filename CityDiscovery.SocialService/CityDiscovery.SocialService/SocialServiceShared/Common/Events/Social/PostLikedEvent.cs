

namespace CityDiscovery.SocialService.SocialServiceShared.Common.Events.Social
{
    public class PostLikedEvent
    {
        public Guid PostId { get; set; }
        public Guid PostAuthorUserId { get; set; } // Post sahibine bildirim gitmesi için
        public Guid UserId { get; set; }           // Beğenen kişi
        public DateTime LikedAt { get; set; }

        public PostLikedEvent(Guid postId, Guid postAuthorUserId, Guid userId, DateTime likedAt)
        {
            PostId = postId;
            PostAuthorUserId = postAuthorUserId;
            UserId = userId;
            LikedAt = likedAt;
        }
    }
}