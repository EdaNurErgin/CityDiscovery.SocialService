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
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
        }

        public async Task<Post> GetByIdAsync(Guid id)
        {
            return await _context.Posts
                .Include(p => p.Comments)
                .Include(p => p.Likes)
                .Include(p => p.Photos)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Post>> GetByVenueIdAsync(Guid venueId)
        {
            return await _context.Posts
                .Where(p => p.VenueId == venueId)
                .Include(p => p.Comments)
                .Include(p => p.Likes)
                .Include(p => p.Photos)
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync();
        }

        //public async Task<List<Post>> GetByUserIdAsync(Guid userId)
        //{
        //    return await _context.Posts
        //        .Where(p => p.UserId == userId)
        //        .ToListAsync();
        //}

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

        public async Task UpdateAuthorDetailsAsync(Guid userId, string newUserName, string newAvatarUrl)
        {
            // Kullanıcıya ait tüm postları bul ve güncelle (Bulk update alternatifi)
            // Not: ExecuteUpdateAsync burada da kullanılabilir ama logic karmaşıksa bu yöntem kalabilir.
            var posts = await _context.Posts
                .Where(p => p.UserId == userId)
                .ToListAsync();

            if (posts.Any())
            {
                foreach (var post in posts)
                {
                    post.AuthorUserName = newUserName;
                    post.AuthorAvatarUrl = newAvatarUrl;
                }
                await _context.SaveChangesAsync();
            }
        }



        // PostRepository sınıfının içine ekleyin:

        public async Task UpdateVenueDetailsAsync(Guid venueId, string newVenueName, string newVenueImageUrl)
        {
            // Bu mekana ait tüm postları bul
            var posts = await _context.Posts
                .Where(p => p.VenueId == venueId)
                .ToListAsync();

            if (posts.Any())
            {
                foreach (var post in posts)
                {
                    // Denormalize edilmiş alanları güncelle
                    post.VenueName = newVenueName;
                    post.VenueImageUrl = newVenueImageUrl;
                }

                // Toplu kaydet
                await _context.SaveChangesAsync();
            }
        }

        // PostRepository sınıfının içine:

        public async Task DeleteAsync(Guid id)
        {
            // 1. Önce silinecek postu bul
            var post = await _context.Posts.FindAsync(id);

            // 2. Eğer post varsa sil
            if (post != null)
            {
                _context.Posts.Remove(post);

                // 3. Değişiklikleri kaydet
                await _context.SaveChangesAsync();
            }
            // Post zaten yoksa hata vermesine gerek yok, işlem başarılı sayılabilir.
        }


        

        public async Task<List<Post>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Posts
                .Where(p => p.UserId == userId)
                .Include(p => p.Comments)
                .Include(p => p.Likes)
                .Include(p => p.Photos)
                .OrderByDescending(p => p.CreatedDate) // En yeni postlar en üstte görünsün
                .ToListAsync();
        }


    }
}