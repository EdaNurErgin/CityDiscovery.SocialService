using CityDiscovery.SocialService.SocialServiceShared.Common.DTOs.Social;
using MassTransit.Mediator;
using MediatR;

namespace SocialService.Application.Queries.GetPost
{
    public class GetPostQuery : IRequest<PostDto>
    {
        public Guid Id { get; set; }
    }
}