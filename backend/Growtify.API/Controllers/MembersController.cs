using Growtify.Application.Interfaces.Repositories;
using Growtify.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Growtify.API.Controllers
{
    public class MembersController(IMemberRepository memberRepository) : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetAllMembers()
        {
            List<Member> users = await memberRepository.GetMembersAsync();

            return Ok(users);
        }

        [HttpGet("{id}")] // localhost:5001/api/members/{id}
        public async Task<IActionResult> GetMemberById(string id)
        {
            Member? user = await memberRepository.GetMemberByIdAsync(id);

            if (user is null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet("{id}/photos")] // localhost:5001/api/members/{id}/photos
        public async Task<ActionResult<IReadOnlyList<Photo>>> GetMemberPhotos(string id)
        {
            return Ok(await memberRepository.GetPhotosForMemberAsync(id));
        }
    }
}
