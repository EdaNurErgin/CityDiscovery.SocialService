using MediatR;
using SocialService.Application.Interfaces;
using SocialService.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SocialService.Application.Commands.LikePost
{
    public class LikePostHandler : IRequestHandler<LikePostCommand, bool>
    {
        private readonly ILikeRepository _likeRepository;
        private readonly IPostRepository _postRepository;

        public LikePostHandler(ILikeRepository likeRepository, IPostRepository postRepository)
        {
            _likeRepository = likeRepository;
            _postRepository = postRepository;
        }

        public async Task<bool> Handle(LikePostCommand request, CancellationToken cancellationToken)
        {
            // Post'un var olup olmadığını kontrol et
            var post = await _postRepository.GetByIdAsync(request.PostId);
            if (post == null)
            {
                throw new Exception("Post not found");
            }

            // Daha önce beğenilmiş mi kontrol et
            var existingLike = await _likeRepository.GetByPostIdAndUserIdAsync(request.PostId, request.UserId);
            
            if (existingLike != null)
            {
                // Zaten beğenilmişse, beğeniyi kaldır (toggle)
                await _likeRepository.RemoveAsync(existingLike);
                return false; // Beğeni kaldırıldı
            }
            else
            {
                // Yeni beğeni ekle
                var like = new PostLike
                {
                    PostId = request.PostId,
                    UserId = request.UserId,
                    LikedDate = DateTime.UtcNow,
                    CreatedDate = DateTime.UtcNow
                };

                await _likeRepository.AddAsync(like);
                return true; // Beğeni eklendi
            }
        }
    }
}