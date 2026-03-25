using Growtify.Application.Common.Pagination;
using Growtify.Application.DTOs.Message;
using Growtify.Domain.Entities;

namespace Growtify.Application.Interfaces.Repositories
{
    public interface IMessageRepository
    {
        void AddMessage(Message message);
        void DeleteMessage(Message message);
        Task<Message?> GetMessage(string messageId);
        Task<PaginatedResult<MessageDto>> GetMessagesForMember(MessageParams messageParams);
        Task<IReadOnlyList<MessageDto>> GetMessageThread(string currentMemberId, string recipientId);
        Task<bool> SaveChangesAsync();
    }
}
