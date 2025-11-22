using SocialService.Application.Interfaces;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SocialService.Infrastructure.HttpClients
{
    public class IdentityServiceClient : IIdentityServiceClient
    {
        private readonly HttpClient _httpClient;

        public IdentityServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserDto> GetUserAsync(Guid userId)
        {
            try
            {
                // Gerçek senaryoda, IdentityService'in ilgili endpoint'ini çağırırız.
                // Örneğin: var response = await _httpClient.GetAsync($"/api/users/{userId}");
                // return await response.Content.ReadFromJsonAsync<UserDto>();

                // ŞİMDİLİK, IdentityService hazır olmadığı için, test amacıyla null döndürelim.
                return await Task.FromResult<UserDto>(null);
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> CheckUserExistsAsync(Guid userId)
        {
            try
            {
                // Gerçek senaryoda, IdentityService'in ilgili endpoint'ini çağırırız.
                // Örneğin: var response = await _httpClient.GetAsync($"/api/users/{userId}/exists");
                // return response.IsSuccessStatusCode;

                // ŞİMDİLİK, IdentityService hazır olmadığı için, test amacıyla her zaman doğru varsayalım.
                return await Task.FromResult(true);
            }
            catch
            {
                return false;
            }
        }
    }
}