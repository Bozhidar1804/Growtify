using Growtify.API.Extensions;
using Growtify.Application.DTOs.Account;
using Growtify.Application.DTOs.Photo;
using Growtify.Application.Interfaces;
using Growtify.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Growtify.API.Controllers
{
    [Authorize]
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
        public async Task<ActionResult<IReadOnlyList<PhotoDto>>> GetMemberPhotos(string id)
        {
            List<Photo> photos = await memberService.GetPhotosForMemberAsync(id);

            var photosDto = photos.Select(photo => new PhotoDto
            {
                Id = photo.Id,
                Url = photo.Url,
            });

            return Ok(photosDto);
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
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
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

            if (await memberService.SaveAllAsync())
            {
                return CreatedAtAction(nameof(GetMemberById),
                new { id = member.Id },
                new PhotoDto
                {
                    Id = photo.Id,
                    Url = photo.Url
                }
                );
            };

            return BadRequest("Problem adding photo");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var member = await memberService.GetMemberByIdAsync(User.GetMemberId());

            if (member == null) return BadRequest("Cannot get member from token.");

            var photo = member.Photos.SingleOrDefault(x =>  x.Id == photoId);

            if (member.ImageUrl == photo?.Url || photo == null)
            {
                return BadRequest("Cannot set this as main image.");
            }

            member.ImageUrl = photo.Url;
            member.User.ImageUrl = photo.Url;

            if (await memberService.SaveAllAsync()) return NoContent();

            return BadRequest("Problem settings main photo");
        }
    }
}
