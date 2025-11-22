using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialService.Application.Commands.CreatePost;
using SocialService.Application.Queries.GetPost;
using SocialService.Application.Queries.GetPostsByVenue;
using SocialService.Application.Queries.GetPostLikeCount;
using SocialService.Shared.Common.DTOs.Social;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialService.API.Controllers
{
    /// <summary>
    /// Gönderiler API - Sosyal medya gönderilerini yönetir
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    public class PostsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PostsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Yeni bir gönderi oluşturur
        /// </summary>
        /// <param name="command">Gönderi oluşturma komutu (UserId, VenueId, Content ve opsiyonel PhotoUrls içerir)</param>
        /// <returns>Oluşturulan gönderi ID'si</returns>
        /// <response code="201">Gönderi başarıyla oluşturuldu</response>
        /// <response code="400">Geçersiz istek veya mekan bulunamadı</response>
        /// <remarks>
        /// Örnek istek:
        /// 
        ///     POST /api/Posts
        ///     {
        ///         "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "venueId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "content": "Harika bir mekan! Kesinlikle tekrar geleceğim.",
        ///         "photoUrls": [
        ///             "https://example.com/photo1.jpg",
        ///             "https://example.com/photo2.jpg"
        ///         ]
        ///     }
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostCommand command)
        {
            try
            {
                // Gelen isteği (command) MediatR'a gönderiyoruz.
                // MediatR doğru handler'ı (CreatePostHandler) bulup çalıştıracak.
                var postId = await _mediator.Send(command);

                // Başarılı bir şekilde oluşturulduğunda, yeni post'un ID'si ile birlikte 201 Created yanıtı dönüyoruz.
                return CreatedAtAction(nameof(GetPostById), new { id = postId }, new { id = postId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// ID'ye göre bir gönderi getirir
        /// </summary>
        /// <param name="id">Gönderi ID'si</param>
        /// <returns>Gönderi detayları</returns>
        /// <response code="200">Gönderi bulundu</response>
        /// <response code="404">Gönderi bulunamadı</response>
        /// <remarks>
        /// Örnek yanıt:
        /// 
        ///     {
        ///         "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "venueId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "authorUserId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "caption": "Harika bir mekan!",
        ///         "photoUrls": [
        ///             "https://example.com/photo1.jpg"
        ///         ],
        ///         "likeCount": 15,
        ///         "commentCount": 3,
        ///         "createdAt": "2024-01-15T10:30:00Z"
        ///     }
        /// </remarks>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PostDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPostById(Guid id)
        {
            var query = new GetPostQuery { Id = id };
            var post = await _mediator.Send(query);

            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        /// <summary>
        /// Belirli bir mekana ait tüm gönderileri getirir
        /// </summary>
        /// <param name="venueId">Mekan ID'si</param>
        /// <returns>Mekana ait gönderi listesi</returns>
        /// <response code="200">Gönderiler başarıyla getirildi</response>
        /// <remarks>
        /// Örnek yanıt:
        /// 
        ///     [
        ///         {
        ///             "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///             "venueId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///             "authorUserId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///             "caption": "Harika bir mekan!",
        ///             "photoUrls": [],
        ///             "likeCount": 10,
        ///             "commentCount": 2,
        ///             "createdAt": "2024-01-15T10:30:00Z"
        ///         }
        ///     ]
        /// </remarks>
        [HttpGet("by-venue/{venueId}")]
        [ProducesResponseType(typeof(List<PostDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPostsByVenue(Guid venueId)
        {
            var query = new GetPostsByVenueQuery { VenueId = venueId };
            var posts = await _mediator.Send(query);
            return Ok(posts);
        }

        /// <summary>
        /// Belirli bir gönderinin beğeni sayısını getirir
        /// </summary>
        /// <param name="postId">Gönderi ID'si</param>
        /// <returns>Gönderi ID'si ve beğeni sayısı</returns>
        /// <response code="200">Beğeni sayısı başarıyla getirildi</response>
        /// <remarks>
        /// Örnek yanıt:
        /// 
        ///     {
        ///         "postId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "likeCount": 25
        ///     }
        /// </remarks>
        [HttpGet("{postId}/likes/count")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPostLikeCount(Guid postId)
        {
            var likeCount = await _mediator.Send(new GetPostLikeCountQuery { PostId = postId });
            return Ok(new { postId, likeCount });
        }
    }
}
    
