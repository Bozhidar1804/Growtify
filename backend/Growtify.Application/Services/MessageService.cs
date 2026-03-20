using Growtify.Application.Common.Mappings;
using Growtify.Application.Common.Pagination;
using Growtify.Application.DTOs.Message;
using Growtify.Application.Interfaces.Repositories;
using Growtify.Application.Interfaces.Services;
using Growtify.Domain.Entities;

namespace Growtify.Application.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository messageRepository;
        private readonly IMemberRepository memberRepository;

        public MessageService(IMessageRepository messageRepository, IMemberRepository memberRepository)
        {
            this.messageRepository = messageRepository;
            this.memberRepository = memberRepository;
        }

        public async Task<MessageDto?> CreateMessageAsync(string senderId, CreateMessageDto dto)
        {
            Member? sender = await memberRepository.GetMemberByIdAsync(senderId);
            Member? recipient = await memberRepository.GetMemberByIdAsync(dto.RecipientId);

            if (sender == null || recipient == null || sender.Id == recipient.Id)
            {
                return null;
            }

            Message message = new Message
            {
                SenderId = sender.Id,
                RecipientId = recipient.Id,
                Content = dto.Content
            };

            messageRepository.AddMessage(message);

            bool success = await messageRepository.SaveChangesAsync();

            if (!success) return null;

            return message.ToDto();
        }

        public async Task<PaginatedResult<MessageDto>> GetMessagesForMemberAsync(MessageParams messageParams)
        {
            return await messageRepository.GetMessagesForMember(messageParams);
        }

        public async Task<IReadOnlyList<MessageDto>> GetMessageThreadAsync(string currentMemberId, string recipientId)
        {
            return await messageRepository.GetMessageThread(currentMemberId, recipientId);
        }
    }
}
