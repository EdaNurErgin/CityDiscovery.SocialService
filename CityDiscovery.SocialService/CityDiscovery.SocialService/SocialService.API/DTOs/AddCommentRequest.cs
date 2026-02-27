namespace CityDiscovery.SocialService.SocialService.API.DTOs
{
    /// <summary>
    /// Yorum ekleme isteği modeli
    /// </summary>
    public class AddCommentRequest
    {
        /// <summary>
        /// Yorum yazarının kullanıcı ID'si
        /// </summary>
        /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
        

        /// <summary>
        /// Yorum içeriği
        /// </summary>
        /// <example>Harika bir gönderi! Çok beğendim.</example>
        public string Content { get; set; }
    }
}
