using Growtify.Application.Common.Pagination;
using Growtify.Application.DTOs.Message;

namespace Growtify.Application.Interfaces.Services
{
    public interface IMessageService
    {
        Task<MessageDto?> CreateMessageAsync(string senderId, CreateMessageDto dto);
        Task<PaginatedResult<MessageDto>> GetMessagesForMemberAsync(MessageParams messageParams);
        Task<IReadOnlyList<MessageDto>> GetMessageThreadAsync(string currentMemberId, string recipientId);
        Task<bool> DeleteMessageAsync(string messageId, string memberId);
    }
}
