using System;
using System.Threading.Tasks;

namespace SocialService.Application.Interfaces
{
    public interface IIdentityServiceClient
    {
        Task<UserDto> GetUserAsync(Guid userId);
        Task<bool> CheckUserExistsAsync(Guid userId);
    }

    public class UserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
