using Growtify.API.Extensions;
using Growtify.Application.DTOs.Message;
using Growtify.Application.Interfaces.Repositories;
using Growtify.Application.Common.Mappings;
using Growtify.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Growtify.Application.Common.Pagination;

namespace Growtify.API.Controllers
{
    public class MessagesController(IMessageRepository messageRepository, IMemberRepository memberRepository) : BaseApiController
    {
        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            Member? sender = await memberRepository.GetMemberByIdAsync(User.GetMemberId());
            Member? recipient = await memberRepository.GetMemberByIdAsync(createMessageDto.RecipientId);

            if (sender == null ||  recipient == null || sender.Id == recipient.Id)
            {
                return BadRequest("Cannot send this message.");
            }

            Message? message = new Message
            {
                SenderId = sender.Id,
                RecipientId = recipient.Id,
                Content = createMessageDto.Content
            };

            messageRepository.AddMessage(message);

            if (await messageRepository.SaveChangesAsync())
            {
                return message.ToDto();
            }

            return BadRequest("Failed to create message.");
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedResult<MessageDto>>> GetMessagesByContainer([FromQuery] MessageParams messageParams)
        {
            messageParams.MemberId = User.GetMemberId();

            return await messageRepository.GetMessagesForMember(messageParams);
        }

        [HttpGet("thread/{recipientId}")]
        public async Task<ActionResult<IReadOnlyList<MessageDto>>> GetMessageThread(string recipientId)
        {
            return Ok(await messageRepository.GetMessageThread(User.GetMemberId(), recipientId));
        }
    }
}
