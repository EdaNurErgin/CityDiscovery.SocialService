using MassTransit.Mediator;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialService.Application.Commands.CreatePost
{
    /// <summary>
    /// Yeni gönderi oluşturma komutu
    /// </summary>
    public class CreatePostCommand : IRequest<Guid>
    {
        /// <summary>
        /// Gönderi yazarının kullanıcı ID'si
        /// </summary>
        /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gönderi içeriği/açıklaması
        /// </summary>
        /// <example>Harika bir mekan! Kesinlikle tekrar geleceğim.</example>
        public string Content { get; set; }

        /// <summary>
        /// Gönderinin oluşturulduğu mekan ID'si
        /// </summary>
        /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
        public Guid VenueId { get; set; }

        /// <summary>
        /// Opsiyonel fotoğraf URL'leri listesi
        /// </summary>
        /// <example>["https://example.com/photo1.jpg", "https://example.com/photo2.jpg"]</example>
        public List<string> PhotoUrls { get; set; }
    }
}