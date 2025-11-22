using MediatR;
using SocialService.Shared.Common.DTOs.Social;
using System;
using System.Collections.Generic;

namespace SocialService.Application.Queries.GetPostsByVenue
{
    public class GetPostsByVenueQuery : IRequest<List<PostDto>>
    {
        public Guid VenueId { get; set; }
    }
}