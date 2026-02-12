namespace CityDiscovery.SocialService.SocialService.Application.DTOs
{
    public class VenueDto
    {
       
            public Guid Id { get; set; }
            public string Name { get; set; }
            public Guid OwnerId { get; set; }
            public string ProfilePictureUrl { get; set; }

    }
}
