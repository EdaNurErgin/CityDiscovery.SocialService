using Microsoft.EntityFrameworkCore;
using SocialService.Application.Interfaces;
using SocialService.Domain.Entities;
using SocialService.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialService.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly SocialDbContext _context;
        public PostRepository(SocialDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Post post)
        {
            // DbContext'in Posts setine yeni post'u ekle
            await _context.Posts.AddAsync(post);
            // Değişiklikleri veritabanına kaydet
            await _context.SaveChangesAsync();
        }

        public async Task<Post> GetByIdAsync(Guid id)
        {
            // Post'u bulurken, ilişkili olduğu yorumları, beğenileri ve fotoğrafları da getirmesini istiyoruz.
            return await _context.Posts
                .Include(p => p.Comments)
                .Include(p => p.Likes)
                .Include(p => p.Photos)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Post>> GetByVenueIdAsync(Guid venueId)
        {
            // Venue'ye ait post'ları bulurken, ilişkili olduğu yorumları, beğenileri ve fotoğrafları da getirmesini istiyoruz.
            return await _context.Posts
                .Where(p => p.VenueId == venueId)
                .Include(p => p.Comments)
                .Include(p => p.Likes)
                .Include(p => p.Photos)
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync();
        }

        public async Task<List<Post>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Posts
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        public async Task DeletePostsByVenueIdAsync(Guid venueId)
        {
            var posts = await _context.Posts
                .Where(p => p.VenueId == venueId)
                .ToListAsync();

            _context.Posts.RemoveRange(posts);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePostsByUserIdAsync(Guid userId)
        {
            var posts = await _context.Posts
                .Where(p => p.UserId == userId)
                .ToListAsync();

            _context.Posts.RemoveRange(posts);
            await _context.SaveChangesAsync();
        }
    }
}