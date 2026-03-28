using Microsoft.EntityFrameworkCore;
using SocialService.Application.Interfaces;
using SocialService.Domain.Entities;
using SocialService.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialService.Infrastructure.Repositories
{
    public class PostSavedRepository : IPostSavedRepository
    {
        private readonly SocialDbContext _context;
        public PostSavedRepository(SocialDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(PostSaved postSaved)
        {
            await _context.PostSaveds.AddAsync(postSaved);
            await _context.SaveChangesAsync();
        }

        // Kullanıcının bu postu daha önce kaydedip kaydetmediğini kontrol eder
        public async Task<PostSaved> GetByPostAndUserAsync(Guid postId, Guid userId)
        {
            return await _context.PostSaveds
                .FirstOrDefaultAsync(s => s.PostId == postId && s.UserId == userId);
        }

        // Kaydedilen postu favorilerden çıkarır
        public async Task RemoveAsync(PostSaved postSaved)
        {
            _context.PostSaveds.Remove(postSaved);
            await _context.SaveChangesAsync();
        }

        
        public async Task<List<PostSaved>> GetSavedPostsByUserIdAsync(Guid userId)
        {
            return await _context.PostSaveds
                .Where(ps => ps.UserId == userId)
                .Include(ps => ps.Post)
                    .ThenInclude(p => p.Comments)
                .Include(ps => ps.Post)
                    .ThenInclude(p => p.Likes)
                .Include(ps => ps.Post)
                    .ThenInclude(p => p.Photos)
                .OrderByDescending(ps => ps.SavedDate) // En son kaydedilen en üstte görünsün
                .ToListAsync();
        }
    }
}