using System;

namespace CityDiscovery.SocialService.SocialServiceShared.Common.Events.Social
{
    public class CommentAddedEvent
    {
        public Guid CommentId { get; set; }
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
        public string? AuthorUserName { get; set; }
        public string? AuthorAvatarUrl { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        public CommentAddedEvent(Guid commentId, Guid postId, Guid userId, string content, DateTime createdAt)
        {
            CommentId = commentId;
            PostId = postId;
            UserId = userId;
            Content = content;
            CreatedAt = createdAt;
        }
    }
}