using MediatR;
using Microsoft.AspNetCore.Mvc;
using SocialService.Application.Commands.LikePost;
using System;
using System.Threading.Tasks;

namespace SocialService.API.Controllers
{
    [Route("api/posts/{postId}/like")]
    [ApiController]
    public class LikesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LikesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> LikePost(Guid postId, [FromBody] LikePostRequest request)
        {
            // JWT token'dan UserId'yi al (şimdilik request'ten alıyoruz, sonra Claims'den alınabilir)
            var command = new LikePostCommand
            {
                PostId = postId,
                UserId = request.UserId // TODO: HttpContext.User.Claims'den alınmalı
            };

            var isLiked = await _mediator.Send(command);
            
            return Ok(new { 
                postId = postId, 
                userId = request.UserId, 
                isLiked = isLiked 
            });
        }
    }

    public class LikePostRequest
    {
        public Guid UserId { get; set; }
    }
}
