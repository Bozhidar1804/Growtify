using Growtify.API.Extensions;
using Growtify.Application.DTOs.Account;
using Growtify.Application.Interfaces;
using Growtify.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Growtify.API.Controllers
{
    public class MembersController(IMemberService memberService, IPhotoService photoService) : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetAllMembers()
        {
            List<Member> users = await memberService.GetAllAsync();

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
        public async Task<ActionResult<IReadOnlyList<Photo>>> GetMemberPhotos(string id)
        {
            return Ok(await memberService.GetPhotosForMemberAsync(id));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMember(MemberUpdateDto memberUpdateDto)
        {
            var memberId = User.GetMemberId();

            var success = await memberService.UpdateMemberAsync(memberId, memberUpdateDto);

            if (!success)
            {
                return BadRequest("Failed to update member");
            }

            return NoContent();
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<Photo>> AddPhoto([FromForm]IFormFile file)
        {
            var member = await memberService.GetMemberByIdAsync(User.GetMemberId());

            if (member == null) return BadRequest("Cannot update member");

            var result = await photoService.UploadPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                MemberId = User.GetMemberId()
            };

            if (member.ImageUrl == null)
            {
                member.ImageUrl = photo.Url;
                member.User.ImageUrl = photo.Url;
            }

            member.Photos.Add(photo);

            if (await memberService.SaveAllAsync()) return photo;

            return BadRequest("Problem adding photo");
        }
    }
}
