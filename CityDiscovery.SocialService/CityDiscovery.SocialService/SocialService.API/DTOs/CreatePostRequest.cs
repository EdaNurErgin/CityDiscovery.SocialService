using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace SocialService.API.DTOs
{
    public class CreatePostRequest
    {
      
        public Guid VenueId { get; set; }
        public string Content { get; set; }

        // Dosyaları buradan alacağız
        public List<IFormFile>? Photos { get; set; }
    }
}