using Growtify.Application.Common.Pagination;
using Growtify.Application.Interfaces.Repositories;
using Growtify.Domain.Entities;
using Growtify.Infrastructure.Data;
using Growtify.Infrastructure.Helpers;
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
        public async Task<PaginatedResult<Member>> GetMembersAsync(MemberParams memberParams)
        {
            var query = context.Members.AsQueryable();

            query = query.Where(x => x.Id != memberParams.CurrentMemberId);

            if (memberParams.Gender != null)
            {
                query = query.Where(x => x.Gender == memberParams.Gender);
            }

            return await PaginationHelper.CreateAsync(query, memberParams.PageNumber, memberParams.PageSize);
        }
        public async Task<Member?> GetMemberByIdAsync(string memberId)
        {
            return await context.Members
                .Include(m => m.User)
                .Include(m => m.Photos)
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
            //context.Entry(member).State = EntityState.Modified;
            context.Members.Update(member);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<Member?> GetMemberForUpdate(string id)
        {
            return await context.Members
                .Include(m => m.User)
                .SingleOrDefaultAsync(m => m.Id == id);
        }
    }
}
