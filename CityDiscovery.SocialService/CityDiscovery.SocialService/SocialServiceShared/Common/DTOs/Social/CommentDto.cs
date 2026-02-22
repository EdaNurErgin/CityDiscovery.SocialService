

namespace CityDiscovery.SocialService.SocialServiceShared.Common.DTOs.Social
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}