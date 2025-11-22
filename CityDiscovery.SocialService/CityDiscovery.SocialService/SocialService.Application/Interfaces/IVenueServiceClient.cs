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

    public class VenueDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid OwnerId { get; set; }
    }
}