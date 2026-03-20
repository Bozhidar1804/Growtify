using Growtify.Application.Interfaces.Repositories;
using Growtify.Domain.Entities;
using Growtify.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Growtify.Infrastructure.Repositories
{
    public class LikesRepository : ILikesRepository
    {
        private readonly GrowtifyDbContext context;
        public LikesRepository(GrowtifyDbContext context)
        {
            this.context = context;
        }
        public void AddLike(MemberLike like)
        {
            context.Likes.Add(like);
        }

        public void DeleteLike(MemberLike like)
        {
            context.Likes.Remove(like);
        }

        public async Task<IReadOnlyList<string>> GetCurrentMemberLikeIdsAsync(string memberId)
        {
            return await context.Likes
                .Where(x => x.SourceMemberId == memberId)
                .Select(x => x.TargetMemberId)
                .ToListAsync();
        }

        public async Task<MemberLike?> GetMemberLikeAsync(string sourceMemberId, string targetMemberId)
        {
            return await context.Likes
                .FindAsync(sourceMemberId, targetMemberId);
                
        }

        public async Task<IReadOnlyList<Member>> GetMemberLikesAsync(string predicate, string memberId)
        {
            var query = context.Likes.AsQueryable();

            switch(predicate)
            {
                case "liked":
                    return await query
                        .Where(x => x.SourceMemberId == memberId)
                        .Select(x => x.TargetMember)
                        .ToListAsync();
                case "likedBy":
                    return await query
                        .Where(x => x.TargetMemberId == memberId)
                        .Select(x => x.SourceMember)
                        .ToListAsync();
                default: // mutual
                    var likedIds = await GetCurrentMemberLikeIdsAsync(memberId);

                    return await query
                        .Where(x => x.TargetMemberId == memberId && likedIds.Contains(x.SourceMemberId))
                        .Select(x => x.SourceMember)
                        .ToListAsync();

            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }
    }
}
