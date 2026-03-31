using Growtify.Application.Common.Pagination;
using Growtify.Application.DTOs.Account;
using Growtify.Application.DTOs.Photo;
using Growtify.Application.Interfaces.Repositories;
using Growtify.Application.Interfaces.Services;
using Growtify.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Growtify.Application.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository memberRepository;
        private readonly IPhotoService photoService;
        public MemberService(IMemberRepository memberRepository, IPhotoService photoService)
        {
            this.memberRepository = memberRepository;
            this.photoService = photoService;
        }
        public async Task<PaginatedResult<Member>> GetAllMembersAsync(MemberParams memberParams)
        {
            return await this.memberRepository.GetAllMembersAsync(memberParams);
        }

        public async Task<Member?> GetMemberByIdAsync(string id)
        {
            return await this.memberRepository.GetMemberByIdAsync(id);
        }

        public async Task<List<PhotoDto>> GetPhotosForMemberAsync(string memberId)
        {
            var photos = await memberRepository.GetPhotosForMemberAsync(memberId);

            return photos.Select(p => new PhotoDto
            {
                Id = p.Id,
                Url = p.Url
            }).ToList();
        }

        public async Task<PhotoDto?> AddPhotoAsync(string memberId, IFormFile file)
        {
            var member = await memberRepository.GetMemberByIdAsync(memberId);
            if (member == null) return null;

            var result = await photoService.UploadPhotoAsync(file);
            if (result.Error != null) throw new Exception(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                MemberId = memberId
            };

            if (string.IsNullOrEmpty(member.ImageUrl))
            {
                member.ImageUrl = photo.Url;
            }

            member.Photos.Add(photo);

            if (!await memberRepository.SaveChangesAsync()) return null;

            return new PhotoDto
            {
                Id = photo.Id,
                Url = photo.Url
            };
        }

        public async Task<bool> SetMainPhotoAsync(string memberId, int photoId)
        {
            var member = await memberRepository.GetMemberByIdAsync(memberId);
            if (member == null) return false;

            var photo = member.Photos.SingleOrDefault(x => x.Id == photoId);
            if (photo == null || member.ImageUrl == photo.Url) return false;

            member.ImageUrl = photo.Url;

            return await memberRepository.SaveChangesAsync();
        }

        public async Task<bool> DeletePhotoAsync(string memberId, int photoId)
        {
            Member? member = await memberRepository.GetMemberByIdAsync(memberId);
            if (member == null) return false;

            Photo? photo = member.Photos.SingleOrDefault(x => x.Id == photoId);
            if (photo == null || photo.Url == member.ImageUrl) return false;

            if (photo.PublicId != null)
            {
                var result = await photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return false;
            }

            member.Photos.Remove(photo);

            return await memberRepository.SaveChangesAsync();
        }

        public async Task<bool> UpdateMemberAsync(string memberId, MemberUpdateDto dto)
        {
            Member? member = await this.memberRepository.GetMemberForUpdate(memberId);

            if (member == null) return false;

            member.UserName = dto.DisplayName ?? member.UserName;
            member.Description = dto.Description ?? member.Description;
            member.City = dto.City ?? member.City;
            member.Country = dto.Country ?? member.Country;

            memberRepository.UpdateMember(member);

            return await memberRepository.SaveChangesAsync();
        }
    }
}
