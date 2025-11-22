using MediatR;
using System;

namespace SocialService.Application.Commands.LikePost
{
    public class LikePostCommand : IRequest<bool>
    {
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
    }
}