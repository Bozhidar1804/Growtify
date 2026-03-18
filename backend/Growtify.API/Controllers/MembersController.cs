using Growtify.API.Extensions;
using Growtify.Application.Common.Pagination;
using Growtify.Application.DTOs.Account;
using Growtify.Application.DTOs.Photo;
using Growtify.Application.Interfaces.Services;
using Growtify.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Growtify.API.Controllers
{
    [Authorize]
    public class MembersController(IMemberService memberService) : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetAllMembers([FromQuery]MemberParams memberParams)
        {
            memberParams.CurrentMemberId = User.GetMemberId();

            PaginatedResult<Member> users = await memberService.GetAllMembersAsync(memberParams); 

            return Ok(users);
        }

        [HttpGet("{id}")] // localhost:5001/api/members/{id}
        public async Task<IActionResult> GetMemberById(string id)
        {
            Member? user = await memberService.GetMemberByIdAsync(id);

            if (user is null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet("{id}/photos")] // localhost:5001/api/members/{id}/photos
        public async Task<ActionResult<IReadOnlyList<PhotoDto>>> GetMemberPhotos(string id)
        {
            List<PhotoDto> photosDtos = await memberService.GetPhotosForMemberAsync(id);
            return Ok(photosDtos);

        }

        [HttpPut]
        public async Task<IActionResult> UpdateMember(MemberUpdateDto memberUpdateDto)
        {
            var memberId = User.GetMemberId();

            bool success = await memberService.UpdateMemberAsync(memberId, memberUpdateDto);

            if (!success)
            {
                return BadRequest("Failed to update member");
            }

            return NoContent();
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var memberId = User.GetMemberId();

            PhotoDto? photoDto = await memberService.AddPhotoAsync(memberId, file);

            if (photoDto == null)
                return BadRequest("Problem adding photo or member not found");

            return CreatedAtAction(
                nameof(GetMemberById),
                new { id = User.GetMemberId() },
                photoDto
            );
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var memberId = User.GetMemberId();

            bool success = await memberService.SetMainPhotoAsync(memberId, photoId);

            if (!success) return BadRequest("Cannot set this as main image.");

            return NoContent();
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var memberId = User.GetMemberId();

            var success = await memberService.DeletePhotoAsync(memberId, photoId);

            if (!success) return BadRequest("Problem deleting photo");

            return Ok();
        }
    }
}
