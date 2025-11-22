using MediatR;
using SocialService.Application.Interfaces;
using SocialService.Domain.Entities;
using SocialService.Shared.Common.Events.Social;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SocialService.Application.Commands.LikePost
{
    public class LikePostHandler : IRequestHandler<LikePostCommand, bool>
    {
        private readonly ILikeRepository _likeRepository;
        private readonly IPostRepository _postRepository;
        private readonly IMessageBus _messageBus;

        public LikePostHandler(ILikeRepository likeRepository, IPostRepository postRepository, IMessageBus messageBus)
        {
            _likeRepository = likeRepository;
            _postRepository = postRepository;
            _messageBus = messageBus;
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

                // PostLikedEvent'i MessageBus ile yayınla (fire-and-forget)
                var postLikedEvent = new PostLikedEvent(
                    postId: request.PostId,
                    userId: request.UserId,
                    likedAt: like.LikedDate);

                // Fire-and-forget: Hata olsa bile response'u bekletme
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await _messageBus.PublishAsync(postLikedEvent, cancellationToken);
                    }
                    catch
                    {
                        // MessageBus'ta hata olsa bile devam et
                    }
                });

                return true; // Beğeni eklendi
            }
        }
    }
}