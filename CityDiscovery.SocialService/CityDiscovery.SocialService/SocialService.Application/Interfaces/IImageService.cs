using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace SocialService.Application.Interfaces
{
    public interface IImageService
    {
        Task<string> SaveImageAsync(IFormFile file);
    }
}