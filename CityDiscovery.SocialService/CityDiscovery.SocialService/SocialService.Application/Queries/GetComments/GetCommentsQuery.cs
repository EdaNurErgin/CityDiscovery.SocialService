using MediatR;
using SocialService.Application.DTOs;
using System;
using System.Collections.Generic;

namespace SocialService.Application.Queries.GetComments
{
    public class GetCommentsQuery : IRequest<List<CommentDto>>
    {
        public Guid PostId { get; set; }
    }
}