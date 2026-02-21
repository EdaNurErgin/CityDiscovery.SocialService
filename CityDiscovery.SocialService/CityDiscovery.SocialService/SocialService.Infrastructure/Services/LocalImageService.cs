using SocialService.Application.Interfaces;


namespace SocialService.Infrastructure.Services
{
    public class LocalImageService : IImageService
    {
        private readonly IWebHostEnvironment _environment;

        public LocalImageService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> SaveImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            
            string webRootPath = _environment.WebRootPath ?? Path.Combine(_environment.ContentRootPath, "wwwroot");

            
            var uploadsFolder = Path.Combine(webRootPath, "uploads", "posts");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            
            var fileExtension = Path.GetExtension(file.FileName);
            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            
            return $"/uploads/posts/{uniqueFileName}";
        }
    }
}