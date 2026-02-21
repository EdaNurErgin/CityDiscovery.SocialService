using MediatR;


namespace SocialService.Application.Commands.AddComment
{
    public class AddCommentCommand : IRequest<Guid>
    {
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; }
    }
}