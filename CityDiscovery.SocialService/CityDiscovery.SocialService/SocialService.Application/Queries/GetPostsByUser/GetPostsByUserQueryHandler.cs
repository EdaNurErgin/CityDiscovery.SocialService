using CityDiscovery.SocialService.SocialServiceShared.Common.DTOs.Social;
using MediatR;
using SocialService.Application.Interfaces;


namespace SocialService.Application.Queries.GetPostsByUser
{
    public class GetPostsByUserQueryHandler : IRequestHandler<GetPostsByUserQuery, List<PostDto>>
    {
        private readonly IPostRepository _postRepository;

        public GetPostsByUserQueryHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<List<PostDto>> Handle(GetPostsByUserQuery request, CancellationToken cancellationToken)
        {
            var posts = await _postRepository.GetByUserIdAsync(request.UserId);

            return posts.Select(post => new PostDto
            {
                Id = post.Id,
                VenueId = post.VenueId,
                AuthorUserId = post.UserId,
                AuthorUserName = post.AuthorUserName, 
                AuthorAvatarUrl = post.AuthorAvatarUrl, 
                Caption = post.Content, 
                CreatedAt = post.CreatedDate,
                PhotoUrls = post.Photos?.Select(p => p.ImageUrl).ToList() ?? new List<string>(), // 'Url' yerine 'ImageUrl' kullanıldı
                LikeCount = post.Likes?.Count ?? 0,
                CommentCount = post.Comments?.Count ?? 0,
                VenueName = post.VenueName,
                VenueImageUrl = post.VenueImageUrl
            }).ToList();
        }
    }
}