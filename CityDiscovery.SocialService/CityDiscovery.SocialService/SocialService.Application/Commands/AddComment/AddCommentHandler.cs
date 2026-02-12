using MediatR;
using SocialService.Application.Interfaces;
using SocialService.Domain.Entities;
using CityDiscovery.SocialService.SocialServiceShared.Common.Events.Social;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SocialService.Application.Commands.AddComment
{
    public class AddCommentHandler : IRequestHandler<AddCommentCommand, Guid>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;
        private readonly IMessageBus _messageBus;

        public AddCommentHandler(ICommentRepository commentRepository, IPostRepository postRepository, IMessageBus messageBus)
        {
            _commentRepository = commentRepository;
            _postRepository = postRepository;
            _messageBus = messageBus;
        }

        public async Task<Guid> Handle(AddCommentCommand request, CancellationToken cancellationToken)
        {
            // Post'un var olup olmadığını kontrol et
            var post = await _postRepository.GetByIdAsync(request.PostId);
            if (post == null)
            {
                throw new Exception("Post not found");
            }

            var comment = new PostComment
            {
                PostId = request.PostId,
                UserId = request.UserId,
                Content = request.Content,
                CreatedDate = DateTime.UtcNow
            };

            await _commentRepository.AddAsync(comment);

            // CommentAddedEvent'i MessageBus ile yayınla (fire-and-forget)
            var commentAddedEvent = new CommentAddedEvent(
                commentId: comment.Id,
                postId: request.PostId,
                userId: request.UserId,
                content: request.Content,
                createdAt: comment.CreatedDate);

            // Fire-and-forget: Hata olsa bile response'u bekletme
            _ = Task.Run(async () =>
            {
                try
                {
                    await _messageBus.PublishAsync(commentAddedEvent, cancellationToken);
                }
                catch
                {
                    // MessageBus'ta hata olsa bile devam et
                }
            });

            return comment.Id;
        }
    }
}