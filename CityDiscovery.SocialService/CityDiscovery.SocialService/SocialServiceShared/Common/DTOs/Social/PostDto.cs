using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityDiscovery.SocialService.SocialServiceShared.Common.DTOs.Social
{
    /// <summary>
    /// Gönderi için veri transfer nesnesi
    /// </summary>
    public class PostDto
    {
        /// <summary>
        /// Gönderi ID'si
        /// </summary>
        /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
        public Guid Id { get; set; }

        /// <summary>
        /// Gönderinin oluşturulduğu mekan ID'si
        /// </summary>
        /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
        public Guid VenueId { get; set; }

        /// <summary>
        /// Gönderi yazarının kullanıcı ID'si
        /// </summary>
        /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
        public Guid AuthorUserId { get; set; }

        public string AuthorUserName { get; set; }
        public string AuthorAvatarUrl { get; set; }

        /// <summary>
        /// Gönderi açıklaması/içeriği
        /// </summary>
        /// <example>Harika bir mekan! Kesinlikle tekrar geleceğim.</example>
        public string Caption { get; set; }

        /// <summary>
        /// Gönderiyle ilişkili fotoğraf URL'leri listesi
        /// </summary>
        /// <example>["https://example.com/photo1.jpg", "https://example.com/photo2.jpg"]</example>
        public List<string> PhotoUrls { get; set; } = new List<string>();

        /// <summary>
        /// Gönderideki beğeni sayısı
        /// </summary>
        /// <example>15</example>
        public int LikeCount { get; set; }

        /// <summary>
        /// Gönderideki yorum sayısı
        /// </summary>
        /// <example>3</example>
        public int CommentCount { get; set; }

        /// <summary>
        /// Gönderi oluşturulma tarihi ve saati
        /// </summary>
        /// <example>2024-01-15T10:30:00Z</example>
        public DateTime CreatedAt { get; set; }

        public string VenueName { get; set; }      // Mekan Adı
        public string VenueImageUrl { get; set; }  // Mekan Resmi
    }
}