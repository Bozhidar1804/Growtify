using Growtify.API.Extensions;
using Growtify.Application.DTOs.Account;
using Growtify.Application.Interfaces;
using Growtify.Application.Interfaces.Repositories;
using Growtify.Domain.Entities;
using Growtify.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Growtify.API.Controllers
{
    public class MembersController(IMemberService memberService) : BaseApiController
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
            Member? user = await memberService.GetByIdAsync(id);

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
    }
}
