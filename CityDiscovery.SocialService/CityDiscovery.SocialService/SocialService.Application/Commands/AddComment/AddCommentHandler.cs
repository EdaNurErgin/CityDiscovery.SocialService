using MediatR;
using SocialService.Application.Interfaces;
using SocialService.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SocialService.Application.Commands.AddComment
{
    public class AddCommentHandler : IRequestHandler<AddCommentCommand, Guid>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;

        public AddCommentHandler(ICommentRepository commentRepository, IPostRepository postRepository)
        {
            _commentRepository = commentRepository;
            _postRepository = postRepository;
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

            return comment.Id;
        }
    }
}