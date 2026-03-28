using CityDiscovery.SocialService.SocialServiceShared.Common.DTOs.Social;
using MediatR;
using System;
using System.Collections.Generic;

namespace SocialService.Application.Queries.GetSavedPosts
{
    public class GetSavedPostsQuery : IRequest<List<PostDto>>
    {
        public Guid UserId { get; set; }
    }
}