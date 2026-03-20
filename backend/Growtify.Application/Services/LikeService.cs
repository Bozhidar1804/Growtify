using Growtify.Application.Interfaces.Repositories;
using Growtify.Application.Interfaces.Services;
using Growtify.Domain.Entities;

namespace Growtify.Application.Services
{
    public class LikeService : ILikeService
    {
        private readonly ILikesRepository likesRepository;

        public LikeService(ILikesRepository likesRepository)
        {
            this.likesRepository = likesRepository;
        }

        public async Task<bool> ToggleLikeAsync(string sourceMemberId, string targetMemberId)
        {
            if (sourceMemberId == targetMemberId)
                throw new InvalidOperationException("You cannot like yourself");

            MemberLike? existingLike = await likesRepository
                .GetMemberLikeAsync(sourceMemberId, targetMemberId);

            if (existingLike == null)
            {
                MemberLike like = new MemberLike
                {
                    SourceMemberId = sourceMemberId,
                    TargetMemberId = targetMemberId
                };

                likesRepository.AddLike(like);
            }
            else
            {
                likesRepository.DeleteLike(existingLike);
            }

            return await likesRepository.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<string>> GetCurrentMemberLikeIdsAsync(string memberId)
        {
            return await likesRepository.GetCurrentMemberLikeIdsAsync(memberId);
        }

        public async Task<IReadOnlyList<Member>> GetMemberLikesAsync(string predicate, string memberId)
        {
            return await likesRepository.GetMemberLikesAsync(predicate, memberId);
        }
    }
}
