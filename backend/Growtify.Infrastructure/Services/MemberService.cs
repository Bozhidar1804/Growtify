using Growtify.Application.DTOs.Account;
using Growtify.Application.Interfaces;
using Growtify.Application.Interfaces.Repositories;
using Growtify.Domain.Entities;

namespace Growtify.Infrastructure.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository memberRepository;
        public MemberService(IMemberRepository memberRepository)
        {
            this.memberRepository = memberRepository;
        }
        public async Task<List<Member>> GetAllAsync()
        {
            return await this.memberRepository.GetMembersAsync();
        }

        public async Task<Member?> GetByIdAsync(string id)
        {
            return await this.memberRepository.GetMemberByIdAsync(id);
        }

        public async Task<List<Photo>> GetPhotosForMemberAsync(string memberId)
        {
            return await memberRepository.GetPhotosForMemberAsync(memberId);
        }

        public async Task<bool> UpdateMemberAsync(string memberId, MemberUpdateDto dto)
        {
            Member? member = await this.memberRepository.GetMemberForUpdate(memberId);

            if (member == null) return false;

            member.UserName = dto.DisplayName ?? member.UserName;
            member.Description = dto.Description ?? member.Description;
            member.City = dto.City ?? member.City;
            member.Country = dto.Country ?? member.Country;

            member.User.UserName = dto.DisplayName ?? member.User.UserName;

            memberRepository.UpdateMember(member);

            return await memberRepository.SaveChangesAsync();
        }
    }
}
