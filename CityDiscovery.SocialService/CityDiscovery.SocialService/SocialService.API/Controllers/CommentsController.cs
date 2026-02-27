using CityDiscovery.SocialService.SocialService.API.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialService.Application.Commands.AddComment;
using SocialService.Application.DTOs;
using SocialService.Application.Queries.GetComments;
using System.Security.Claims;



namespace SocialService.API.Controllers
{
    /// <summary>
    /// Yorumlar API - Gönderilerdeki yorumları yönetir
    /// </summary>
    [Route("api/posts/{postId}/comments")]
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    public class CommentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CommentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Bir gönderiye yorum ekler
        /// </summary>
        /// <param name="postId">Gönderi ID'si</param>
        /// <param name="request">Yorum isteği (Sadece Content içerir, UserId token'dan alınır)</param>
        /// <returns>Oluşturulan yorum ID'si</returns>
        /// <response code="201">Yorum başarıyla oluşturuldu</response>
        /// <response code="400">Geçersiz istek veya gönderi bulunamadı</response>
        /// <response code="401">Yetkilendirme hatası (Geçersiz Token)</response>
        /// <remarks>
        /// Örnek istek:
        /// 
        ///     POST /api/posts/{postId}/comments
        ///     {
        ///         "content": "Harika bir gönderi! Çok beğendim."
        ///     }
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // EKLENDİ
        public async Task<IActionResult> AddComment(Guid postId, [FromBody] AddCommentRequest request)
        {
            try
            {
                // Kimliği token içerisinden güvenli bir şekilde çekiyoruz
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                               ?? User.FindFirst("sub")?.Value;

                if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
                {
                    return Unauthorized(new { error = "Kullanıcı kimliği doğrulanamadı." });
                }

                var command = new AddCommentCommand
                {
                    PostId = postId,
                    UserId = userId, // DOĞRU: Token'dan gelen ID'yi kullanıyoruz
                    Content = request.Content
                };

                var commentId = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetComments), new { postId }, new { id = commentId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Belirli bir gönderiye ait tüm yorumları getirir
        /// </summary>
        /// <param name="postId">Gönderi ID'si</param>
        /// <returns>Yorum listesi</returns>
        /// <response code="200">Yorumlar başarıyla getirildi</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<CommentDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetComments(Guid postId)
        {
            var query = new GetCommentsQuery { PostId = postId };
            var comments = await _mediator.Send(query);
            return Ok(comments);
        }
    }
}


