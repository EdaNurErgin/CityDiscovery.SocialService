using CityDiscovery.SocialService.SocialServiceShared.Common.DTOs.Social;
using MediatR;
using SocialService.Application.Interfaces;


namespace SocialService.Application.Queries.GetPost
{
    public class GetPostHandler : IRequestHandler<GetPostQuery, PostDto>
    {
        private readonly IPostRepository _postRepository;
        public GetPostHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        public async Task<PostDto> Handle(GetPostQuery request, CancellationToken cancellationToken)
        {
            var post = await _postRepository.GetByIdAsync(request.Id);

            if (post == null)
            {

                return null;
            }


            // Post varlığını PostDto'ya dönüştürüyoruz (map'leme).
            var postDto = new PostDto
            {
                Id = post.Id,
                VenueId = post.VenueId,
                AuthorUserId = post.UserId,
                AuthorUserName = post.AuthorUserName,
                AuthorAvatarUrl = post.AuthorAvatarUrl,
                VenueName = post.VenueName, 
                VenueImageUrl = post.VenueImageUrl,

                Caption = post.Content,
                PhotoUrls = post.Photos?.Select(p => p.ImageUrl).ToList() ?? new List<string>(),
                LikeCount = post.Likes?.Count ?? 0,
                CommentCount = post.Comments?.Count ?? 0,
                CreatedAt = post.CreatedDate
            };

            return postDto;
        }
    }
}