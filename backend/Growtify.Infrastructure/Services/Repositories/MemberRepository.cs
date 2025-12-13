using Growtify.Application.Interfaces.Repositories;
using Growtify.Domain.Entities;
using Growtify.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Growtify.Infrastructure.Services.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly GrowtifyDbContext context;

        public MemberRepository(GrowtifyDbContext context)
        {
            this.context = context;
        }
        public async Task<List<Member>> GetMembersAsync()
        {
            return await context.Members
                //.Include(m => m.Photos)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<Member?> GetMemberByIdAsync(string memberId)
        {
            return await context.Members
                //.Include(m => m.Photos)
                .FirstOrDefaultAsync(m => m.Id == memberId);
        }

        public async Task<List<Photo>> GetPhotosForMemberAsync(string memberId)
        {
            return await context.Members.Where(m => m.Id == memberId)
                                        .SelectMany(m => m.Photos)
                                        .AsNoTracking()
                                        .ToListAsync();
        }

        public void UpdateMember(Member member)
        {
            context.Entry(member).State = EntityState.Modified;
        }
    }
}
