using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialService.Application.Commands.AddComment;
using SocialService.Application.Queries.GetComments;
using SocialService.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        /// <param name="request">Yorum isteği (UserId ve Content içerir)</param>
        /// <returns>Oluşturulan yorum ID'si</returns>
        /// <response code="201">Yorum başarıyla oluşturuldu</response>
        /// <response code="400">Geçersiz istek veya gönderi bulunamadı</response>
        /// <remarks>
        /// Örnek istek:
        /// 
        ///     POST /api/posts/{postId}/comments
        ///     {
        ///         "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "content": "Harika bir gönderi! Çok beğendim."
        ///     }
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddComment(Guid postId, [FromBody] AddCommentRequest request)
        {
            try
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
        /// <remarks>
        /// Örnek yanıt:
        /// 
        ///     [
        ///         {
        ///             "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///             "postId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///             "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///             "content": "Harika bir gönderi!",
        ///             "createdDate": "2024-01-15T10:30:00Z",
        ///             "updatedDate": null
        ///         }
        ///     ]
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(typeof(List<CommentDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetComments(Guid postId)
        {
            var query = new GetCommentsQuery { PostId = postId };
            var comments = await _mediator.Send(query);
            return Ok(comments);
        }
    }

    /// <summary>
    /// Yorum ekleme isteği modeli
    /// </summary>
    public class AddCommentRequest
    {
        /// <summary>
        /// Yorum yazarının kullanıcı ID'si
        /// </summary>
        /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
        public Guid UserId { get; set; }

        /// <summary>
        /// Yorum içeriği
        /// </summary>
        /// <example>Harika bir gönderi! Çok beğendim.</example>
        public string Content { get; set; }
    }
}