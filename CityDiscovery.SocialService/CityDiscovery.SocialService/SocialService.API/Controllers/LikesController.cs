using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialService.Application.Commands.LikePost;
using System;
using System.Security.Claims;
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
        /// <returns>Gönderi ID'si, Kullanıcı ID'si ve gönderinin beğenilip beğenilmediği</returns>
        /// <response code="200">Beğeni durumu başarıyla güncellendi</response>
        /// <response code="400">Geçersiz istek veya gönderi bulunamadı</response>
        /// <response code="401">Yetkilendirme hatası (Geçersiz Token)</response>
        /// <remarks>
        /// Örnek istek:
        /// 
        ///     POST /api/posts/{postId}/like
        ///     (Body boş gönderilir, UserId token'dan otomatik alınır)
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
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> LikePost(Guid postId) // [FromBody] parametresi tamamen kaldırıldı
        {
            try
            {
                // JWT token'dan UserId'yi alıyoruz
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                               ?? User.FindFirst("sub")?.Value;

                if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
                {
                    return Unauthorized(new { error = "Kullanıcı kimliği doğrulanamadı." });
                }

                // Token'dan aldığımız ID'yi komuta veriyoruz
                var command = new LikePostCommand
                {
                    PostId = postId,
                    UserId = userId
                };

                
                var isLiked = await _mediator.Send(command);

                return Ok(new
                {
                    postId = postId,
                    userId = userId,
                    isLiked = isLiked
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}