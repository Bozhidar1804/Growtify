using Growtify.Application.Common.Pagination;
using Growtify.Application.DTOs.Account;
using Growtify.Application.DTOs.Photo;
using Growtify.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Growtify.Application.Interfaces.Services
{
    public interface IMemberService
    {
        Task<bool> UpdateMemberAsync(string memberId, MemberUpdateDto dto);
        Task<Member?> GetMemberByIdAsync(string id);
        Task<PaginatedResult<Member>> GetAllMembersAsync(MemberParams memberParams);
        Task<PhotoDto?> AddPhotoAsync(string memberId, IFormFile file);
        Task<bool> SetMainPhotoAsync(string memberId, int photoId);
        Task<bool> DeletePhotoAsync(string memberId, int photoId);
        Task<List<PhotoDto>> GetPhotosForMemberAsync(string memberId);
    }
}
