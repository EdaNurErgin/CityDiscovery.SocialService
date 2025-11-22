using MediatR;
using SocialService.Application.Interfaces;
using SocialService.Shared.Common.DTOs.Social;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SocialService.Application.Queries.GetPostsByVenue
{
    public class GetPostsByVenueHandler : IRequestHandler<GetPostsByVenueQuery, List<PostDto>>
    {
        private readonly IPostRepository _postRepository;

        public GetPostsByVenueHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<List<PostDto>> Handle(GetPostsByVenueQuery request, CancellationToken cancellationToken)
        {
            var posts = await _postRepository.GetByVenueIdAsync(request.VenueId);

            return posts.Select(post => new PostDto
            {
                Id = post.Id,
                VenueId = post.VenueId,
                AuthorUserId = post.UserId,
                Caption = post.Content,
                PhotoUrls = post.Photos?.Select(p => p.ImageUrl).ToList() ?? new List<string>(),
                LikeCount = post.Likes?.Count ?? 0,
                CommentCount = post.Comments?.Count ?? 0,
                CreatedAt = post.CreatedDate
            }).ToList();
        }
    }
}