using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialService.Application.Commands.LikePost;
using System;
using System.Threading.Tasks;

namespace SocialService.API.Controllers
{
    /// <summary>
    /// Beğeniler API - Gönderilerdeki beğenileri yönetir
    /// </summary>
    [Route("api/posts/{postId}/like")]
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    public class LikesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LikesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Bir gönderiyi beğenir veya beğeniyi kaldırır (toggle)
        /// </summary>
        /// <param name="postId">Gönderi ID'si</param>
        /// <param name="request">Beğeni isteği (UserId içerir)</param>
        /// <returns>Gönderi ID'si, Kullanıcı ID'si ve gönderinin beğenilip beğenilmediği</returns>
        /// <response code="200">Beğeni durumu başarıyla güncellendi</response>
        /// <response code="400">Geçersiz istek veya gönderi bulunamadı</response>
        /// <remarks>
        /// Örnek istek:
        /// 
        ///     POST /api/posts/{postId}/like
        ///     {
        ///         "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
        ///     }
        ///     
        /// Örnek yanıt (beğenildi):
        /// 
        ///     {
        ///         "postId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "isLiked": true
        ///     }
        ///     
        /// Örnek yanıt (beğeni kaldırıldı):
        /// 
        ///     {
        ///         "postId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "isLiked": false
        ///     }
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LikePost(Guid postId, [FromBody] LikePostRequest request)
        {
            try
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
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }

    /// <summary>
    /// Gönderi beğenme isteği modeli
    /// </summary>
    public class LikePostRequest
    {
        /// <summary>
        /// Gönderiyi beğenen kullanıcının ID'si
        /// </summary>
        /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
        public Guid UserId { get; set; }
    }
}
