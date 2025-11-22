using MediatR;
using SocialService.Application.DTOs;
using SocialService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SocialService.Application.Queries.GetComments
{
    public class GetCommentsHandler : IRequestHandler<GetCommentsQuery, List<CommentDto>>
    {
        private readonly ICommentRepository _commentRepository;

        public GetCommentsHandler(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<List<CommentDto>> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
        {
            var comments = await _commentRepository.GetByPostIdAsync(request.PostId);

            return comments.Select(c => new CommentDto
            {
                Id = c.Id,
                PostId = c.PostId,
                UserId = c.UserId,
                Content = c.Content,
                CreatedDate = c.CreatedDate,
                UpdatedDate = c.LastModifiedDate
            }).ToList();
        }
    }
}