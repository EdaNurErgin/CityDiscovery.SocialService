using CityDiscovery.SocialService.SocialService.Application.DTOs;
using System;
using System.Threading.Tasks;

namespace SocialService.Application.Interfaces
{
    public interface IIdentityServiceClient
    {
        Task<UserDto> GetUserAsync(Guid userId);
        Task<bool> CheckUserExistsAsync(Guid userId);
    }


}
