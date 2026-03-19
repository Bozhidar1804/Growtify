using Growtify.API.Extensions;
using Growtify.Application.Interfaces.Repositories;
using Growtify.Application.Interfaces.Services;
using Growtify.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Growtify.API.Controllers
{
    public class LikesController(ILikeService likeService) : BaseApiController
    {
        [HttpPost("{targetMemberId}")]
        public async Task<ActionResult> ToggleLike(string targetMemberId)
        {
            string sourceMemberId = User.GetMemberId();

            try
            {
                bool success = await likeService.ToggleLikeAsync(sourceMemberId, targetMemberId);

                if (success) return Ok();

                return BadRequest("Failed to update like.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("list")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetCurrentMemberLikeIds()
        {
            return Ok(await likeService.GetCurrentMemberLikeIdsAsync(User.GetMemberId()));
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Member>>> GetMemberLikes(string predicate)
        {
            IReadOnlyList<Member> result = await likeService.GetMemberLikesAsync(predicate, User.GetMemberId());
            return Ok(result);
        }
    }
}
