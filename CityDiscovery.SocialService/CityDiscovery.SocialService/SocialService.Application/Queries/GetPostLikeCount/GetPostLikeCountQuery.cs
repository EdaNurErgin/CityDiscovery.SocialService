using MediatR;
using System;

namespace SocialService.Application.Queries.GetPostLikeCount
{
    public class GetPostLikeCountQuery : IRequest<int>
    {
        public Guid PostId { get; set; }
    }
}

