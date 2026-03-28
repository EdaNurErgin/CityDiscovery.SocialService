using CityDiscovery.SocialService.SocialServiceShared.Common.DTOs.Social;
using MediatR;
using SocialService.Application.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SocialService.Application.Queries.GetSavedPosts
{
    public class GetSavedPostsHandler : IRequestHandler<GetSavedPostsQuery, List<PostDto>>
    {
        private readonly IPostSavedRepository _postSavedRepository;

        public GetSavedPostsHandler(IPostSavedRepository postSavedRepository)
        {
            _postSavedRepository = postSavedRepository;
        }

        public async Task<List<PostDto>> Handle(GetSavedPostsQuery request, CancellationToken cancellationToken)
        {
            // Repository üzerinden kaydedilen kayıtları (ve içindeki Post'ları) çekiyoruz
            var savedPosts = await _postSavedRepository.GetSavedPostsByUserIdAsync(request.UserId);

            // Gelen PostSaved listesindeki Post nesnelerini PostDto'ya map'liyoruz
            return savedPosts.Select(sp => new PostDto
            {
                Id = sp.Post.Id,
                VenueId = sp.Post.VenueId,
                AuthorUserId = sp.Post.UserId,
                AuthorUserName = sp.Post.AuthorUserName,
                AuthorAvatarUrl = sp.Post.AuthorAvatarUrl,
                Caption = sp.Post.Content,
                PhotoUrls = sp.Post.Photos?.Select(p => p.ImageUrl).ToList() ?? new List<string>(),
                LikeCount = sp.Post.Likes?.Count ?? 0,
                CommentCount = sp.Post.Comments?.Count ?? 0,
                CreatedAt = sp.Post.CreatedDate,
                VenueName = sp.Post.VenueName,
                VenueImageUrl = sp.Post.VenueImageUrl
            }).ToList();
        }
    }
}