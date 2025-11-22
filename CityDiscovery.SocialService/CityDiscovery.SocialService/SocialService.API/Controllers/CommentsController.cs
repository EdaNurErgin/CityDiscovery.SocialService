using MediatR;
using Microsoft.AspNetCore.Mvc;
using SocialService.Application.Commands.AddComment;
using SocialService.Application.Queries.GetComments;
using System;
using System.Threading.Tasks;

namespace SocialService.API.Controllers
{
    [Route("api/posts/{postId}/comments")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CommentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(Guid postId, [FromBody] AddCommentRequest request)
        {
            // JWT token'dan UserId'yi al (şimdilik request'ten alıyoruz, sonra Claims'den alınabilir)
            var command = new AddCommentCommand
            {
                PostId = postId,
                UserId = request.UserId, // TODO: HttpContext.User.Claims'den alınmalı
                Content = request.Content
            };

            var commentId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetComments), new { postId }, new { id = commentId });
        }

        [HttpGet]
        public async Task<IActionResult> GetComments(Guid postId)
        {
            var query = new GetCommentsQuery { PostId = postId };
            var comments = await _mediator.Send(query);
            return Ok(comments);
        }
    }

    public class AddCommentRequest
    {
        public Guid UserId { get; set; }
        public string Content { get; set; }
    }
}