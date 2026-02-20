using CityDiscovery.SocialService.SocialServiceShared.Common.DTOs.Social;
using MediatR;


namespace SocialService.Application.Queries.GetPostsByUser
{
    public class GetPostsByUserQuery : IRequest<List<PostDto>>
    {
        public Guid UserId { get; set; }
    }
}