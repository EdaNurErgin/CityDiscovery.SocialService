using SocialService.Application.Interfaces;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SocialService.Infrastructure.HttpClients
{
    public class VenueServiceClient : IVenueServiceClient
    {
        private readonly HttpClient _httpClient;
        
        public VenueServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public async Task<bool> VenueExistsAsync(Guid venueId)
        {
            return await CheckVenueExistsAsync(venueId);
        }

        public async Task<bool> CheckVenueExistsAsync(Guid venueId)
        {
            try
            {
                // Gerçek senaryoda, VenueService'in ilgili endpoint'ini çağırırız.
                // Örneğin: var response = await _httpClient.GetAsync($"/api/venues/{venueId}");
                // return response.IsSuccessStatusCode;

                // ŞİMDİLİK, VenueService hazır olmadığı için, test amacıyla her zaman doğru varsayalım.
                return await Task.FromResult(true);
            }
            catch
            {
                // Hata durumunda mekan yokmuş gibi davranabiliriz.
                return false;
            }
        }

        public async Task<VenueDto> GetVenueAsync(Guid venueId)
        {
            try
            {
                // Gerçek senaryoda, VenueService'in ilgili endpoint'ini çağırırız.
                // Örneğin: var response = await _httpClient.GetAsync($"/api/venues/{venueId}");
                // return await response.Content.ReadFromJsonAsync<VenueDto>();

                // ŞİMDİLİK, VenueService hazır olmadığı için, test amacıyla null döndürelim.
                return await Task.FromResult<VenueDto>(null);
            }
            catch
            {
                return null;
            }
        }

        public async Task<Guid> GetVenueOwnerAsync(Guid venueId)
        {
            try
            {
                // Gerçek senaryoda, VenueService'in ilgili endpoint'ini çağırırız.
                // Örneğin: var venue = await GetVenueAsync(venueId);
                // return venue?.OwnerId ?? Guid.Empty;

                // ŞİMDİLİK, VenueService hazır olmadığı için, test amacıyla boş Guid döndürelim.
                return await Task.FromResult(Guid.Empty);
            }
            catch
            {
                return Guid.Empty;
            }
        }
    }
}