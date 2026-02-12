using CityDiscovery.SocialService.SocialService.Application.DTOs;
using System;
using System.Threading.Tasks;

namespace SocialService.Application.Interfaces
{
    public interface IVenueServiceClient
    {
        Task<bool> VenueExistsAsync(Guid venueId);
        Task<bool> CheckVenueExistsAsync(Guid venueId);
        Task<VenueDto> GetVenueAsync(Guid venueId);
        Task<Guid> GetVenueOwnerAsync(Guid venueId);
    }


}