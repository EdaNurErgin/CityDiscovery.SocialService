using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialService.Application.DTOs
{
    /// <summary>
    /// Yorum için veri transfer nesnesi
    /// </summary>
    public class CommentDto
    {
        /// <summary>
        /// Yorum ID'si
        /// </summary>
        /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
        public Guid Id { get; set; }

        /// <summary>
        /// Yorumun ait olduğu gönderi ID'si
        /// </summary>
        /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
        public Guid PostId { get; set; }

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

        /// <summary>
        /// Yorum oluşturulma tarihi ve saati
        /// </summary>
        /// <example>2024-01-15T10:30:00Z</example>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Yorum son güncelleme tarihi ve saati (güncellenmişse)
        /// </summary>
        /// <example>2024-01-15T11:00:00Z</example>
        public DateTime? UpdatedDate { get; set; }
    }
}