using SocialService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialService.Application.Interfaces
{
    public interface IPostRepository
    {
        Task AddAsync(Post post);
        Task<Post> GetByIdAsync(Guid id);
        Task<List<Post>> GetByVenueIdAsync(Guid venueId);
        Task<List<Post>> GetByUserIdAsync(Guid userId);
        Task DeletePostsByVenueIdAsync(Guid venueId);
        Task DeletePostsByUserIdAsync(Guid userId);
    }
}