using Growtify.API.Extensions;
using Growtify.Application.DTOs.Message;
using Microsoft.AspNetCore.Mvc;
using Growtify.Application.Common.Pagination;
using Growtify.Application.Interfaces.Services;

namespace Growtify.API.Controllers
{
    public class MessagesController(IMessageService messageService) : BaseApiController
    {
        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            MessageDto? result = await messageService.CreateMessageAsync(User.GetMemberId(), createMessageDto);

            if (result == null)
            {
                return BadRequest("Cannot send this message.");
            }

            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedResult<MessageDto>>> GetMessagesByContainer([FromQuery] MessageParams messageParams)
        {
            messageParams.MemberId = User.GetMemberId();

            return Ok(await messageService.GetMessagesForMemberAsync(messageParams));
        }

        [HttpGet("thread/{recipientId}")]
        public async Task<ActionResult<IReadOnlyList<MessageDto>>> GetMessageThread(string recipientId)
        {
            var result = await messageService.GetMessageThreadAsync(User.GetMemberId(), recipientId);

            return Ok(result);
        }
    }
}
