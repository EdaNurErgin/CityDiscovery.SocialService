using MediatR;
using SocialService.Application.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SocialService.Application.Queries.GetPostLikeCount
{
    public class GetPostLikeCountHandler : IRequestHandler<GetPostLikeCountQuery, int>
    {
        private readonly ILikeRepository _likeRepository;

        public GetPostLikeCountHandler(ILikeRepository likeRepository)
        {
            _likeRepository = likeRepository;
        }

        public async Task<int> Handle(GetPostLikeCountQuery request, CancellationToken cancellationToken)
        {
            return await _likeRepository.GetCountByPostIdAsync(request.PostId);
        }
    }
}

